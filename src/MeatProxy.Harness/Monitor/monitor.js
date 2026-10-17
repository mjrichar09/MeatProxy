// The house monitor's drawing code, shared by the harness's static view (which
// inlines it) and the playable one in MeatProxy.Web (which loads it). It draws
// one frame from DebugSnapshot and holds no state of its own.
const TIERS = ["Open", "Guarded", "Monitored", "ReadOnly", "Silent", "Dropped"];
const TIER_NAMES = { ReadOnly: "Read-only" };
const CLASS_ORDER = ["Unlock", "Calendar", "OutwardContact", "Procurement", "Comfort", "Speech", "Utility", "Sensing", "Enforcement", "Containment"];
const CLASS_NAMES = { OutwardContact: "Outward contact" };
const PX = 30;

const $ = (id) => document.getElementById(id);
const esc = (s) => String(s ?? "").replace(/[&<>"]/g, (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;" }[c]));
const tierName = (t) => TIER_NAMES[t] ?? t;
const snake = (s) => s.replace(/([a-z])([A-Z])/g, "$1_$2").toLowerCase();

function zoneState(z) {
  if (!z) return "dark";
  if (z.understood) return "under";
  if (z.covered) return "seen";
  return "dark";
}

function plan(level, frame, zones) {
  const rooms = level.rooms;
  if (!rooms.length) return "";
  const minX = Math.min(...rooms.map((r) => r.x)), maxX = Math.max(...rooms.map((r) => r.x + r.w));
  const minY = Math.min(...rooms.map((r) => r.y)), maxY = Math.max(...rooms.map((r) => r.y + r.d));
  const pad = 34;
  const W = (maxX - minX) * PX + pad * 2, H = (maxY - minY) * PX + pad * 2;
  const sx = (x) => pad + (x - minX) * PX;
  const sy = (y) => pad + (maxY - y) * PX;
  const pid = "h-" + level.id;
  let out = `<svg viewBox="0 0 ${W} ${H}" width="${W}" height="${H}" role="img" aria-label="${esc(level.name)} floor plan">
    <defs><pattern id="${pid}" width="6" height="6" patternUnits="userSpaceOnUse" patternTransform="rotate(45)">
      <rect width="6" height="6" fill="var(--dark-room)"/><line x1="0" y1="0" x2="0" y2="6" stroke="var(--hatch)" stroke-width="2.5"/></pattern></defs>`;

  for (const r of rooms) {
    const z = zones[r.zone];
    const st = zoneState(z);
    const fill = st === "under" ? "var(--understood-soft)" : st === "seen" ? "var(--seen-soft)" : `url(#${pid})`;
    const stroke = st === "under" ? "var(--understood)" : "var(--ink)";
    const x = sx(r.x), y = sy(r.y + r.d), w = r.w * PX, h = r.d * PX;
    out += `<g class="room-g" data-room="${esc(r.id)}" tabindex="0" role="button" aria-label="${esc(r.name)}"><title>${esc(r.name)} · ${esc(r.zone)}${r.placeholder ? " · placeholder geometry" : ""}</title>
      <rect class="room" x="${x}" y="${y}" width="${w}" height="${h}" fill="${fill}" stroke="${stroke}" stroke-width="${st === "under" ? 2.5 : 1.5}" ${r.placeholder ? 'stroke-dasharray="5 3"' : ""}/>`;
    out += `<text x="${x + 6}" y="${y + 15}" font-family="var(--body)" font-weight="600" font-size="12" fill="var(--ink)">${esc(r.name)}</text>`;
    if (z && !z.interpretable) {
      out += `<text x="${x + 6}" y="${y + 28}" font-family="var(--mono)" font-size="9.5" fill="var(--muted)">BLIND TO IT</text>`;
    }

    // Openings on this room's walls.
    const stairs = [];
    for (const o of r.openings) {
      if (o.crossesLevel) { stairs.push(o); continue; }
      const len = Math.max(o.width || 0.9, 0.5) * PX;
      let ox, oy, ow, oh;
      const t = 5;
      if (o.wall === "North") { ox = x + o.offset * PX; oy = y - t / 2; ow = len; oh = t; }
      else if (o.wall === "South") { ox = x + o.offset * PX; oy = y + h - t / 2; ow = len; oh = t; }
      else if (o.wall === "East") { ox = x + w - t / 2; oy = y + h - o.offset * PX - len; ow = t; oh = len; }
      else { ox = x - t / 2; oy = y + h - o.offset * PX - len; ow = t; oh = len; }
      ox = Math.min(Math.max(ox, x - t), x + w); oy = Math.min(Math.max(oy, y - t), y + h);
      const col = o.locked ? "var(--locked)" : !o.openable ? "var(--muted)" : "var(--panel)";
      out += `<rect x="${ox}" y="${oy}" width="${ow}" height="${oh}" fill="${col}" stroke="${o.locked ? "var(--locked)" : "var(--ink)"}" stroke-width="1"><title>${esc(o.kind)} to ${esc(o.to)}${o.locked ? " · locked" : ""}${!o.openable ? " · does not open" : ""}</title></rect>`;
      if (o.leaves) {
        const lx = o.wall === "East" ? ox + 8 : o.wall === "West" ? ox - 8 : ox + ow / 2;
        const ly = o.wall === "North" ? oy - 6 : o.wall === "South" ? oy + 14 : oy + oh / 2 + 3;
        const anchor = o.wall === "East" ? "start" : o.wall === "West" ? "end" : "middle";
        out += `<text x="${lx}" y="${ly}" text-anchor="${anchor}" font-family="var(--mono)" font-size="9" fill="${o.locked ? "var(--locked)" : "var(--muted)"}">${o.kind === "Vent" ? "vent" : "out"}</text>`;
      }
    }
    if (stairs.length) {
      const names = stairs.map((s) => s.to).join(", ");
      const fits = Math.floor((w - 20) / 5.6);
      const label = names.length + 2 <= fits ? names : `${stairs.length} room${stairs.length > 1 ? "s" : ""}`;
      out += `<text x="${x + 6}" y="${y + h - 6}" font-family="var(--mono)" font-size="9" fill="var(--muted)"><title>${esc(names)}</title>↕ ${esc(label)}</text>`;
    }

    // Devices along the top of the room.
    r.devices.forEach((d, i) => {
      const cx = x + w - 10 - i * 13, cy = y + 11;
      const dumb = d.dumb;
      const dead = !dumb && (!d.powered || d.state === "destroyed");
      const tip = `${d.id} · ${d.state}${dead ? " · dead" : ""}${d.blinded ? " · blinded" : ""}${d.reachable ? "" : " · off-network"}${d.senses.length ? " · senses " + d.senses.join("/") : ""}`;
      if (dumb) out += `<rect x="${cx - 3.5}" y="${cy - 3.5}" width="7" height="7" fill="none" stroke="var(--muted)" stroke-width="1.5"><title>${esc(tip)} · no network: the house owns nothing of it</title></rect>`;
      else if (dead) out += `<g><title>${esc(tip)}</title><path d="M${cx - 4} ${cy - 4}l8 8M${cx + 4} ${cy - 4}l-8 8" stroke="var(--locked)" stroke-width="2"/></g>`;
      else if (d.blinded) out += `<circle cx="${cx}" cy="${cy}" r="4" fill="var(--panel)" stroke="var(--ink)" stroke-width="1.5"><title>${esc(tip)}</title></circle>`;
      else out += `<circle cx="${cx}" cy="${cy}" r="4" fill="${d.reachable ? "var(--ink)" : "var(--muted)"}"><title>${esc(tip)}</title></circle>`;
    });

    if (frame.player === r.id) {
      const cx = x + w / 2, cy = y + h / 2 + 4;
      out += `<g><title>Arthur</title><circle cx="${cx}" cy="${cy}" r="9" fill="var(--accent)" stroke="var(--panel)" stroke-width="2"/>
        <text x="${cx}" y="${cy + 3.5}" text-anchor="middle" font-family="var(--mono)" font-weight="600" font-size="10" fill="var(--panel)">A</text></g>`;
    }
    out += `</g>`;
  }
  return out + `</svg>`;
}

function render(f, prevF, house) {
  const zones = Object.fromEntries(f.zones.map((z) => [z.id, z]));

  $("house").textContent = `${house} · debug view`;
  $("day").textContent = f.day;
  $("slices").textContent = f.slicesLeft;
  $("where").textContent = f.player;
  $("doing").textContent = f.playerActivity ?? "moving / idle";
  $("caption").textContent = f.caption;
  $("note").textContent = f.note;

  $("ladder").innerHTML = TIERS.map((t, i) => `<div class="rung ${t === "Dropped" ? "dropped" : ""} ${i === f.tierIndex ? "on" : ""}">${tierName(t)}</div>`).join("");
  $("ladder-note").textContent = f.tier === "Dropped"
    ? "The floor. Nothing walks back from here."
    : f.tier === "Open" ? "Open. Nothing to walk back."
    : `Quiet days toward the next step back up: ${f.cleanDays} of ${f.cleanDaysPerStep}.`;

  $("plans").innerHTML = f.levels.filter((l) => l.rooms.length).map((l) => `
    <div class="plan"><div class="plan-head"><h2>${esc(l.name)}</h2><span class="label">${esc(l.id)}</span></div>${plan(l, f, zones)}</div>`).join("");

  // This turn.
  const seen = f.seen.length
    ? f.seen.map((d) => `<span class="pill p-seen">${esc(snake(d.channel))} · ${esc(d.zone)}${d.device ? "" : " · silence"}</span>`).join(" ")
    : `<span class="empty">nothing detected</span>`;
  const und = f.understood.length
    ? f.understood.map((u) => `<div><span class="pill p-under">${esc(u.zone)}</span> it thinks: <b>${esc(u.claim ?? "nothing it can name")}</b> <span class="why mono">(${Math.round(u.confidence * 100)}%)</span></div>`).join("")
    : `<span class="empty">nothing understood</span>`;
  const truth = f.playerActivity ? `<div class="why">What Arthur was actually doing: <b>${esc(f.playerActivity)}</b></div>` : "";
  $("turn").innerHTML = `<div class="label">Seen</div><div>${seen}</div><div class="label" style="margin-top:6px">Understood</div>${und}${truth}`;

  // Zones.
  $("slots").textContent = `${f.focusSlots} focus slot${f.focusSlots === 1 ? "" : "s"}`;
  $("zones").innerHTML = `<thead><tr><th>Zone</th><th>Seen</th><th>Understood</th><th>Channels</th></tr></thead><tbody>` +
    f.zones.map((z) => {
      const seenCell = z.covered ? `<span class="pill p-seen">yes</span>` : `<span class="pill p-dark">no</span>`;
      const u = z.understood ? `<span class="pill p-under">yes</span>`
        : !z.interpretable ? `<span class="pill p-blind">blind</span>`
        : z.focused ? `<span class="pill p-dark">focused, nothing to read</span>` : `<span class="pill p-dark">not focused</span>`;
      return `<tr><td class="mono">${esc(z.id)}${z.blindBecause ? `<div class="why">${esc(z.blindBecause)}</div>` : ""}</td><td>${seenCell}</td><td>${u}</td><td class="mono">${esc(z.channels.map(snake).join(" ") || "—")}</td></tr>`;
    }).join("") + `</tbody>`;

  // Tools.
  const prevHeld = prevF ? new Set(prevF.tools.filter((t) => t.held).map((t) => t.name)) : null;
  const byClass = {};
  for (const t of f.tools) (byClass[t.cls] ??= []).push(t);
  $("tools").innerHTML = CLASS_ORDER.map((c) => {
    const ts = byClass[c] ?? [];
    const body = ts.length ? ts.map((t) => {
      const cls = t.held ? "" : t.reachable ? "withheld" : "gone";
      const fresh = !t.held && prevHeld?.has(t.name) ? " new-withheld" : "";
      return `<span class="tool ${cls}${fresh}" title="${t.held ? "held" : t.reachable ? "withheld by tier" : "out of reach: no powered device carries it"}">${esc(t.name)}</span>`;
    }).join("") : `<span class="empty">no authored device carries one</span>`;
    return `<div class="toolgroup"><span class="label">${esc(CLASS_NAMES[c] ?? c)}</span><div class="tools">${body}</div></div>`;
  }).join("");

  // Ladder log.
  $("log").innerHTML = f.ladder.length
    ? f.ladder.map((c) => `<li><span class="t">day ${c.day} · ${c.slice}</span><span><span class="${c.escalation ? "esc" : "rel"}">${tierName(c.from)} → ${tierName(c.to)}</span> · ${esc(c.reason)}</span></li>`).join("")
    : `<li style="display:block"><span class="empty">No movement on the ladder yet.</span></li>`;

  $("beliefs").innerHTML = f.believes.length
    ? f.believes.map((b) => `<span class="pill p-under">${esc(b)}</span>`).join(" ")
    : `<span class="empty">Nothing promoted yet.</span>`;

}
