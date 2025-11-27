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
  removeLocal: function (key) {
    try { localStorage.removeItem(key); } catch {}
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
    try {
      // Align with CSS variable used by app.css
      document.documentElement.style.setProperty('--accent', color);
    } catch {}
  },
  setTheme: function (theme) {
    try { document.documentElement.setAttribute('data-theme', theme); } catch {}
  },
  sha256: async function (message) {
    const enc = new TextEncoder();
    const data = enc.encode(message);
    const hash = await crypto.subtle.digest('SHA-256', data);
    return Array.from(new Uint8Array(hash)).map(b => b.toString(16).padStart(2, '0')).join('');
  },
  copyToClipboard: async function (text) {
    try {
      await navigator.clipboard.writeText(text || '');
      return true;
    } catch (e) {
      console.error('copyToClipboard error', e);
      return false;
    }
  },
  setCookie: function (name, value, days) {
    var expires = "";
    if (days) {
      var date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/; samesite=lax";
  }
};
