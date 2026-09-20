let _canvas = null;
let _ctx = null;
let _dotnet = null;
let _size = 0;
let _dpr = 1;
let _dragging = false;

let _h = 0, _s = 100, _v = 100;
let _wheelCanvas = null;
let _wheelCtx = null;

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
    buildHueWheel();

    canvas.addEventListener('pointerdown', onDown);
    canvas.addEventListener('pointermove', onMove);
    canvas.addEventListener('pointerup', onUp);
    canvas.addEventListener('pointercancel', onUp);
    canvas.addEventListener('pointerleave', onUp);
}
function buildHueWheel() {
    _wheelCanvas =
        document.createElement("canvas");

    _wheelCanvas.width = _size;
    _wheelCanvas.height = _size;

    _wheelCtx =
        _wheelCanvas.getContext("2d");

    const R = _size / 2;

    const outerR = R - 2;
    const innerR = R * INNER_R_RATIO;

    for (let i = 0; i < 360; i++) {
        const a0 =
            i * Math.PI / 180;

        const a1 =
            (i + 1) * Math.PI / 180;

        _wheelCtx.beginPath();

        _wheelCtx.arc(
            R,
            R,
            outerR,
            a0,
            a1);

        _wheelCtx.arc(
            R,
            R,
            innerR,
            a1,
            a0,
            true);

        _wheelCtx.closePath();

        _wheelCtx.fillStyle =
            `hsl(${i},100%,50%)`;

        _wheelCtx.fill();
    }
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
    _h = h;
    _s = s;
    _v = v;

    if (!_ctx)
        return;

    const size = _size;
    const R = size / 2;

    const outerR = R - 2;
    const innerR = R * INNER_R_RATIO;

    const squareSize =
        innerR * 1.35;

    const sx =
        R - squareSize / 2;

    const sy =
        R - squareSize / 2;

    _ctx.clearRect(
        0,
        0,
        size,
        size);

    // hue wheel

    _ctx.drawImage(
        _wheelCanvas,
        0,
        0);

    // SV square

    _ctx.save();

    _ctx.beginPath();

    _ctx.arc(
        R,
        R,
        innerR,
        0,
        Math.PI * 2);

    _ctx.clip();

    const satGradient =
        _ctx.createLinearGradient(
            sx,
            sy,
            sx + squareSize,
            sy);

    satGradient.addColorStop(
        0,
        "#ffffff");

    satGradient.addColorStop(
        1,
        `hsl(${h},100%,50%)`);

    _ctx.fillStyle =
        satGradient;

    _ctx.fillRect(
        sx,
        sy,
        squareSize,
        squareSize);

    const valueGradient =
        _ctx.createLinearGradient(
            sx,
            sy,
            sx,
            sy + squareSize);

    valueGradient.addColorStop(
        0,
        "rgba(0,0,0,0)");

    valueGradient.addColorStop(
        1,
        "rgba(0,0,0,1)");

    _ctx.fillStyle =
        valueGradient;

    _ctx.fillRect(
        sx,
        sy,
        squareSize,
        squareSize);

    _ctx.restore();

    _ctx.strokeStyle =
        "rgba(0,0,0,0.25)";

    _ctx.lineWidth = 1;

    _ctx.strokeRect(
        sx,
        sy,
        squareSize,
        squareSize);

    // hue marker

    const hueAngle =
        h * Math.PI / 180;

    const hueRadius =
        (outerR + innerR) / 2;

    const hx =
        R +
        Math.cos(hueAngle) *
        hueRadius;

    const hy =
        R +
        Math.sin(hueAngle) *
        hueRadius;

    _ctx.beginPath();

    _ctx.arc(
        hx,
        hy,
        7,
        0,
        Math.PI * 2);

    _ctx.strokeStyle =
        "#000";

    _ctx.lineWidth = 3;

    _ctx.stroke();

    _ctx.beginPath();

    _ctx.arc(
        hx,
        hy,
        7,
        0,
        Math.PI * 2);

    _ctx.strokeStyle =
        "#fff";

    _ctx.lineWidth = 1.5;

    _ctx.stroke();

    // SV marker

    const px =
        sx +
        (_s / 100) *
        squareSize;

    const py =
        sy +
        (1 - _v / 100) *
        squareSize;

    _ctx.beginPath();

    _ctx.arc(
        px,
        py,
        7,
        0,
        Math.PI * 2);

    _ctx.fillStyle =
        hsvToCss(
            _h,
            _s,
            _v);

    _ctx.fill();

    _ctx.lineWidth = 2;
    _ctx.strokeStyle = "#fff";
    _ctx.stroke();

    _ctx.lineWidth = 1;
    _ctx.strokeStyle = "#000";
    _ctx.stroke();
}
function hsvToCss(h, s, v) {
    s /= 100;
    v /= 100;

    const c = v * s;

    const x =
        c *
        (1 - Math.abs((h / 60) % 2 - 1));

    const m =
        v - c;

    let r = 0;
    let g = 0;
    let b = 0;

    if (h < 60) {
        r = c;
        g = x;
    }
    else if (h < 120) {
        r = x;
        g = c;
    }
    else if (h < 180) {
        g = c;
        b = x;
    }
    else if (h < 240) {
        g = x;
        b = c;
    }
    else if (h < 300) {
        r = x;
        b = c;
    }
    else {
        r = c;
        b = x;
    }

    r = Math.round((r + m) * 255);
    g = Math.round((g + m) * 255);
    b = Math.round((b + m) * 255);

    return `rgb(${r},${g},${b})`;
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