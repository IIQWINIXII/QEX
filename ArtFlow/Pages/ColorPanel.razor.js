let _canvas = null;
let _ctx = null;
let _dotnet = null;
let _size = 0;
let _dpr = 1;
let _dragging = false;

let _h = 0, _s = 100, _v = 100;

const OUTER_R_RATIO = 1.0;
const INNER_R_RATIO = 0.78;

export function init(canvas, dotnetRef) {
    _canvas = canvas;
    _ctx = canvas.getContext('2d');
    _dotnet = dotnetRef;
    _dpr = window.devicePixelRatio || 1;

    _size = Math.min(canvas.width, canvas.height);

    canvas.width = _size * _dpr;
    canvas.height = _size * _dpr;
    canvas.style.width = _size + 'px';
    canvas.style.height = _size + 'px';
    _ctx.setTransform(_dpr, 0, 0, _dpr, 0, 0);

    canvas.addEventListener('pointerdown', onDown);
    canvas.addEventListener('pointermove', onMove);
    canvas.addEventListener('pointerup', onUp);
    canvas.addEventListener('pointercancel', onUp);
    canvas.addEventListener('pointerleave', onUp);
}

function onDown(e) {
    _dragging = true;
    try { _canvas.setPointerCapture(e.pointerId); } catch (_) { }
    dispatch(e);
}

function onMove(e) {
    if (!_dragging) return;
    dispatch(e);
}

function onUp(e) {
    _dragging = false;
    try { _canvas.releasePointerCapture(e.pointerId); } catch (_) { }
}

function dispatch(e) {
    const rect = _canvas.getBoundingClientRect();
    const cx = rect.left + rect.width / 2;
    const cy = rect.top + rect.height / 2;
    const nx = (e.clientX - cx) / (rect.width / 2);
    const ny = (e.clientY - cy) / (rect.height / 2);

    _dotnet.invokeMethodAsync('OnJsPointer', nx, ny, _dragging);
}

export function redraw(h, s, v) {
    _h = h; _s = s; _v = v;
    if (!_ctx) return;

    const size = _size;
    const R = size / 2;

    _ctx.clearRect(0, 0, size, size);

    // 1. Кольцо Hue
    const outerR = R * OUTER_R_RATIO - 2;
    const innerR = R * INNER_R_RATIO;

    const steps = 360;
    for (let i = 0; i < steps; i++) {
        const a0 = (i / steps) * Math.PI * 2;
        const a1 = ((i + 1) / steps) * Math.PI * 2;

        _ctx.beginPath();
        _ctx.arc(R, R, outerR, a0, a1);
        _ctx.arc(R, R, innerR, a1, a0, true);
        _ctx.closePath();
        _ctx.fillStyle = `hsl(${i}, 100%, 50%)`;
        _ctx.fill();
    }

    // 2. Треугольник SV
    const tr = innerR * 0.98;
    const cos30 = Math.cos(Math.PI / 6);
    const sin30 = 0.5;

    const Ax = R, Ay = R - tr;
    const Bx = R - tr * cos30, By = R + tr * sin30;
    const Cx = R + tr * cos30, Cy = R + tr * sin30;

    const gAB = _ctx.createLinearGradient(Ax, Ay, Bx, By);
    gAB.addColorStop(0, `hsl(${h}, 100%, 50%)`);
    gAB.addColorStop(1, '#ffffff');

    _ctx.save();
    _ctx.beginPath();
    _ctx.moveTo(Ax, Ay);
    _ctx.lineTo(Bx, By);
    _ctx.lineTo(Cx, Cy);
    _ctx.closePath();
    _ctx.clip();

    _ctx.fillStyle = gAB;
    _ctx.fillRect(R - tr, R - tr, tr * 2, tr * 2);

    const gAC = _ctx.createLinearGradient(Ax, Ay, Cx, Cy);
    gAC.addColorStop(0, 'rgba(0,0,0,0)');
    gAC.addColorStop(1, 'rgba(0,0,0,1)');
    _ctx.fillStyle = gAC;
    _ctx.fillRect(R - tr, R - tr, tr * 2, tr * 2);

    _ctx.restore();

    // Обводка треугольника
    _ctx.beginPath();
    _ctx.moveTo(Ax, Ay);
    _ctx.lineTo(Bx, By);
    _ctx.lineTo(Cx, Cy);
    _ctx.closePath();
    _ctx.strokeStyle = 'rgba(0,0,0,0.25)';
    _ctx.lineWidth = 1;
    _ctx.stroke();

    // 3. Маркер Hue
    const hueAngle = (h * Math.PI) / 180;
    const hueR = (outerR + innerR) / 2;
    const hx = R + Math.cos(hueAngle) * hueR;
    const hy = R + Math.sin(hueAngle) * hueR;

    _ctx.beginPath();
    _ctx.arc(hx, hy, 7, 0, Math.PI * 2);
    _ctx.strokeStyle = '#000';
    _ctx.lineWidth = 3;
    _ctx.stroke();

    _ctx.beginPath();
    _ctx.arc(hx, hy, 7, 0, Math.PI * 2);
    _ctx.strokeStyle = '#fff';
    _ctx.lineWidth = 1.5;
    _ctx.stroke();

    // 4. Маркер SV
    const sv = s / 100;
    const vv = v / 100;
    const wa = sv * vv;
    const wb = (1 - sv) * vv;
    const wc = 1 - vv;

    const px = wa * Ax + wb * Bx + wc * Cx;
    const py = wa * Ay + wb * By + wc * Cy;

    _ctx.beginPath();
    _ctx.arc(px, py, 6, 0, Math.PI * 2);
    _ctx.strokeStyle = '#000';
    _ctx.lineWidth = 3;
    _ctx.stroke();

    _ctx.beginPath();
    _ctx.arc(px, py, 6, 0, Math.PI * 2);
    _ctx.strokeStyle = '#fff';
    _ctx.lineWidth = 1.5;
    _ctx.stroke();
}

export function dispose() {
    if (_canvas) {
        _canvas.removeEventListener('pointerdown', onDown);
        _canvas.removeEventListener('pointermove', onMove);
        _canvas.removeEventListener('pointerup', onUp);
        _canvas.removeEventListener('pointercancel', onUp);
        _canvas.removeEventListener('pointerleave', onUp);
    }
    _canvas = null;
    _ctx = null;
    _dotnet = null;
}