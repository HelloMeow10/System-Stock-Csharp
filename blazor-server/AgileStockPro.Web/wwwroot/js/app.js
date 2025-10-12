window.app = {
  downloadFile: function (filename, content, mimeType) {
    try {
      const blob = new Blob([content], { type: mimeType || 'text/plain;charset=utf-8' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = filename;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch (e) {
      console.error('downloadFile error', e);
    }
  },
  setLocal: function (key, value) {
    try { localStorage.setItem(key, value); } catch { }
  },
  getLocal: function (key) {
    try { return localStorage.getItem(key); } catch { return null; }
  },
  setLocalJson: function (key, obj) {
    try { localStorage.setItem(key, JSON.stringify(obj)); } catch {}
  },
  getLocalJson: function (key) {
    try {
      const v = localStorage.getItem(key);
      return v ? JSON.parse(v) : null;
    } catch { return null; }
  },
  setAccent: function (color) {
    try { document.documentElement.style.setProperty('--accent-color', color); } catch {}
  },
  setTheme: function (theme) {
    try { document.documentElement.setAttribute('data-theme', theme); } catch {}
  },
  sha256: async function (message) {
    const enc = new TextEncoder();
    const data = enc.encode(message);
    const hash = await crypto.subtle.digest('SHA-256', data);
    return Array.from(new Uint8Array(hash)).map(b => b.toString(16).padStart(2, '0')).join('');
  }
};
