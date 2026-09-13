// Pages/Draw.js
let ctx = null;              // композитный контекст видимого канваса
let canvasEl = null;
let stageEl = null;

// --- Слои ---
// порядок: [0] — самый нижний, [n-1] — самый верхний
let layers = [];             // [{ id, canvas, visible, opacity, blendMode }]
let activeLayerId = null;

// --- Рисование ---
let isDrawing = false;
let startX = 0, startY = 0;
let savedData = null;
let tool = 'pencil';
let color = '#000000';
let size = 3;
let background = '#FFFFFF';

// --- Зум ---
let zoom = 1.0;
let panX = 0, panY = 0;
const ZOOM_MIN = 0.05;
const ZOOM_MAX = 16.0;
const ZOOM_FACTOR = 1.12;

/* ============================================================
 *  Утилиты
 * ============================================================ */

function makeOffscreen(w, h) {
    const c = document.createElement('canvas');
    c.width = w;
    c.height = h;
    return c;
}

function getActiveLayer() {
    return layers.find(l => l.id === activeLayerId) || null;
}

function fillBackground() {
    if (!ctx) return;
    ctx.clearRect(0, 0, canvasEl.width, canvasEl.height);
    if (background && background !== 'transparent') {
        ctx.fillStyle = background;
        ctx.fillRect(0, 0, canvasEl.width, canvasEl.height);
    }
}

function clamp(v, a, b) { return v < a ? a : v > b ? b : v; }

/* ============================================================
 *  Композит
 * ============================================================ */

export function composite() {
    if (!ctx) return;

    ctx.save();
    ctx.globalAlpha = 1;
    ctx.globalCompositeOperation = 'source-over';
    ctx.clearRect(0, 0, canvasEl.width, canvasEl.height);
    if (background && background !== 'transparent') {
        ctx.fillStyle = background;
        ctx.fillRect(0, 0, canvasEl.width, canvasEl.height);
    }
    ctx.restore();

    for (const layer of layers) {
        if (!layer.visible || layer.opacity <= 0) continue;
        ctx.save();
        ctx.globalAlpha = layer.opacity;
        ctx.globalCompositeOperation = layer.blendMode || 'source-over';
        ctx.drawImage(layer.canvas, 0, 0);
        ctx.restore();
    }
}

/* ============================================================
 *  Инициализация / resize
 * ============================================================ */

export function init(canvas, stage, bg) {
    if (!canvas || !stage) return;
    canvasEl = canvas;
    stageEl = stage;
    ctx = canvas.getContext('2d');
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;

    if (layers.length === 0) {
        const id = 'layer-' + Math.random().toString(36).slice(2, 10);
        layers.push({
            id,
            canvas: makeOffscreen(canvas.width, canvas.height),
            visible: true,
            opacity: 1,
            blendMode: 'source-over'
        });
        activeLayerId = id;
    }

    composite();
    centerCanvas();
}

export function resize(w, h, bg) {
    if (!ctx) return;
    canvasEl.width = w;
    canvasEl.height = h;
    if (typeof bg === 'string') background = bg;

    // пересоздаём буферы слоёв под новый размер (содержимое сбрасывается)
    for (const l of layers) {
        l.canvas = makeOffscreen(w, h);
    }

    zoom = 1.0;
    centerCanvas();
    composite();
}

/**
 * Пересоздать слои по списку ids (используется после resize,
 * если родитель уже знает актуальный набор слоёв).
 */
export function reinitLayers(ids, activeId) {
    if (!ctx) return;
    const map = new Map(layers.map(l => [l.id, l]));
    const next = [];
    for (const id of ids) {
        const existing = map.get(id);
        if (existing) {
            existing.canvas = makeOffscreen(canvasEl.width, canvasEl.height);
            next.push(existing);
        } else {
            next.push({
                id,
                canvas: makeOffscreen(canvasEl.width, canvasEl.height),
                visible: true,
                opacity: 1,
                blendMode: 'source-over'
            });
        }
    }
    layers = next;
    activeLayerId = activeId || (layers[0]?.id ?? null);
    composite();
}

/* ============================================================
 *  API слоёв
 * ============================================================ */

export function addLayer(id) {
    layers.push({
        id,
        canvas: makeOffscreen(canvasEl.width, canvasEl.height),
        visible: true,
        opacity: 1,
        blendMode: 'source-over'
    });
    activeLayerId = id;
    composite();
}

export function deleteLayer(id) {
    layers = layers.filter(l => l.id !== id);
    if (activeLayerId === id) {
        activeLayerId = layers.length ? layers[layers.length - 1].id : null;
    }
    composite();
}

export function duplicateLayer(srcId, newId) {
    const src = layers.find(l => l.id === srcId);
    if (!src) return;
    const copy = makeOffscreen(canvasEl.width, canvasEl.height);
    copy.getContext('2d').drawImage(src.canvas, 0, 0);
    const idx = layers.findIndex(l => l.id === srcId);
    layers.splice(idx + 1, 0, {
        id: newId,
        canvas: copy,
        visible: src.visible,
        opacity: src.opacity,
        blendMode: src.blendMode
    });
    activeLayerId = newId;
    composite();
}

export function setActiveLayer(id) {
    activeLayerId = id;
}

export function setLayerVisible(id, visible) {
    const l = layers.find(x => x.id === id);
    if (!l) return;
    l.visible = !!visible;
    composite();
}

export function setActiveLayerProps(opacity, blendMode) {
    const l = getActiveLayer();
    if (!l) return;
    l.opacity = opacity;
    l.blendMode = blendMode;
    composite();
}

export function setLayerOrder(idsInOrder) {
    const map = new Map(layers.map(l => [l.id, l]));
    const reordered = [];
    for (const id of idsInOrder) {
        const l = map.get(id);
        if (l) reordered.push(l);
    }
    for (const l of layers) {
        if (!idsInOrder.includes(l.id)) reordered.push(l);
    }
    layers = reordered;
    composite();
}

/* ============================================================
 *  Рисование (в активный слой)
 * ============================================================ */

function toCanvas(clientX, clientY) {
    const rect = canvasEl.getBoundingClientRect();
    const scaleX = canvasEl.width / rect.width;
    const scaleY = canvasEl.height / rect.height;
    return {
        x: (clientX - rect.left) * scaleX,
        y: (clientY - rect.top) * scaleY
    };
}

export function start(clientX, clientY) {
    const layer = getActiveLayer();
    if (!layer) return;

    const { x, y } = toCanvas(clientX, clientY);
    isDrawing = true;
    startX = x;
    startY = y;

    const lctx = layer.canvas.getContext('2d');
    lctx.lineCap = 'round';
    lctx.lineJoin = 'round';
    savedData = lctx.getImageData(0, 0, layer.canvas.width, layer.canvas.height);
    lctx.beginPath();
    lctx.moveTo(x, y);
}

export function move(clientX, clientY) {
    if (!isDrawing) return;
    const layer = getActiveLayer();
    if (!layer) return;

    const lctx = layer.canvas.getContext('2d');
    const { x, y } = toCanvas(clientX, clientY);
    lctx.putImageData(savedData, 0, 0);
    lctx.strokeStyle = color;
    lctx.lineWidth = size;
    lctx.lineCap = 'round';
    lctx.lineJoin = 'round';

    if (tool === 'pencil') {
        lctx.lineTo(x, y);
        lctx.stroke();
    } else if (tool === 'line') {
        lctx.beginPath();
        lctx.moveTo(startX, startY);
        lctx.lineTo(x, y);
        lctx.stroke();
    } else if (tool === 'rect') {
        lctx.strokeRect(startX, startY, x - startX, y - startY);
    } else if (tool === 'circle') {
        const r = Math.max(Math.abs(x - startX), Math.abs(y - startY));
        lctx.beginPath();
        lctx.arc(startX, startY, r, 0, 2 * Math.PI);
        lctx.stroke();
    }

    composite();
}

export function end() {
    isDrawing = false;
    savedData = null;
}

export function clear() {
    const layer = getActiveLayer();
    if (!layer) return;
    const lctx = layer.canvas.getContext('2d');
    lctx.clearRect(0, 0, layer.canvas.width, layer.canvas.height);
    composite();
}

export function sync(t, c, s) {
    tool = t;
    color = c;
    size = s;
}

export function toDataUrl() {
    if (!canvasEl) return '';
    return canvasEl.toDataURL('image/png');
}

/* ============================================================
 *  Зум / панорама
 * ============================================================ */

function centerCanvas() {
    if (!canvasEl || !stageEl) return;
    const sw = stageEl.clientWidth;
    const sh = stageEl.clientHeight;
    const cw = canvasEl.width;
    const ch = canvasEl.height;
    panX = Math.round((sw - cw) / 2);
    panY = Math.round((sh - ch) / 2);
    applyTransform();
}

function applyTransform() {
    if (!canvasEl) return;
    canvasEl.style.transform =
        `translate(${panX}px, ${panY}px) scale(${zoom})`;
    canvasEl.style.width = canvasEl.width + 'px';
    canvasEl.style.height = canvasEl.height + 'px';
}

let wheelHandler = null;

export function hookWheel(stage) {
    if (!stage) return;
    stageEl = stage;
    if (wheelHandler) return;

    wheelHandler = function (e) {
        e.preventDefault();

        const stageRect = stageEl.getBoundingClientRect();
        const mx = e.clientX - stageRect.left;
        const my = e.clientY - stageRect.top;

        const bufX = (mx - panX) / zoom;
        const bufY = (my - panY) / zoom;

        const factor = e.deltaY < 0 ? ZOOM_FACTOR : 1 / ZOOM_FACTOR;
        const newZoom = clamp(zoom * factor, ZOOM_MIN, ZOOM_MAX);
        if (Math.abs(newZoom - zoom) < 1e-6) return;
        zoom = newZoom;

        panX = mx - bufX * zoom;
        panY = my - bufY * zoom;
        applyTransform();
    };

    stageEl.addEventListener('wheel', wheelHandler, { passive: false });
}

export function unhookWheel() {
    if (stageEl && wheelHandler) {
        stageEl.removeEventListener('wheel', wheelHandler, { passive: false });
    }
    wheelHandler = null;
}

export function dispose() {
    ctx = null;
    canvasEl = null;
    stageEl = null;
    layers = [];
    activeLayerId = null;
    isDrawing = false;
    savedData = null;
    zoom = 1.0;
    panX = 0;
    panY = 0;
    wheelHandler = null;
}