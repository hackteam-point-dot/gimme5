import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'

const globalStyles = `
  .ring-dialog-dialog {
    max-width: 100vw !important;
    max-height: 100vh !important;
    overflow-y: auto !important;
    box-sizing: border-box !important;
    padding: 16px !important;
  }
  .ring-island-island {
    max-width: 100% !important;
    box-sizing: border-box !important;
    overflow-x: hidden !important;
  }
  .ring-tabs-tabs {
    display: flex !important;
    flex-wrap: wrap !important;
    gap: 8px !important;
    max-width: 100% !important;
    padding-bottom: 2px !important;
  }
`;

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <style dangerouslySetInnerHTML={{ __html: globalStyles }} />
    <App />
  </React.StrictMode>,
)
