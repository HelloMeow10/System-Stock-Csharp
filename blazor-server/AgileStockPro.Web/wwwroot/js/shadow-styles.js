export function applyVentasShadowStyles() {
  try {
    const rootStyles = getComputedStyle(document.documentElement);
    const bg = rootStyles.getPropertyValue('--neutral-layer-2').trim() || '#131c2e';
    const fg = rootStyles.getPropertyValue('--neutral-foreground-rest').trim() || '#e5e7eb';
    const stroke = rootStyles.getPropertyValue('--stroke-color').trim() || '#243042';
    const bgPanel = rootStyles.getPropertyValue('--neutral-layer-1').trim() || '#0e1626';

    // Date pickers / pickers
    document.querySelectorAll('.page-ventas fluent-date-picker, .page-ventas fluent-datepicker, .page-ventas fluent-picker').forEach(el => {
      try {
        const s = el.shadowRoot;
        if (s) {
          if (!s.getElementById('ventas-dark-fix-style')) {
            const style = document.createElement('style');
            style.id = 'ventas-dark-fix-style';
            style.textContent = `
              input, [part="control"], [part="input"], [part="textbox"], .control, .text-field {
                background: ${bg} !important;
                color: ${fg} !important;
                border: 1px solid ${stroke} !important;
                border-radius: 8px !important;
              }
              [part="calendar"], [part="popup"] { background: ${bg} !important; color: ${fg} !important; }
            `;
            s.appendChild(style);
          }
        } else {
          // fallback: set host CSS variables if available
          el.style.setProperty('--neutral-layer-2', bg);
          el.style.setProperty('--neutral-foreground-rest', fg);
          el.style.setProperty('--stroke-color', stroke);
        }
      } catch (e) { /* ignore per-element errors */ }
    });

    // Data grid
    document.querySelectorAll('.page-ventas fluent-data-grid').forEach(el => {
      try {
        const s = el.shadowRoot;
        if (s) {
          if (!s.getElementById('ventas-datagrid-dark-fix-style')) {
            const style = document.createElement('style');
            style.id = 'ventas-datagrid-dark-fix-style';
            style.textContent = `
              ::part(column-header), ::part(header-cell) { background: ${bg} !important; color: ${fg} !important; }
              ::part(cell), ::part(row-cell) { background: ${bgPanel} !important; color: ${fg} !important; border-bottom: 1px solid ${stroke} !important; }
              ::part(filter) { background: ${bg} !important; color: ${fg} !important; }
            `;
            s.appendChild(style);
          }
        } else {
          // fallback: set host CSS variables
          el.style.setProperty('--neutral-layer-1', bgPanel);
          el.style.setProperty('--neutral-foreground-rest', fg);
        }
      } catch (e) { /* ignore */ }
    });
  } catch (err) {
    // swallow errors to avoid breaking app
    console.warn('applyVentasShadowStyles failed', err);
  }
}
