import { useState } from 'react';
import { DashboardWidget } from './DashboardWidget';
import { AdminPanel } from './AdminPanel';
import { FlappyBug } from './FlappyBug';

function App() {
  const [appMode, setAppMode] = useState<'user' | 'admin' | 'minigame'>('user');

  return (
    <div style={{ 
      fontFamily: 'Inter, system-ui, sans-serif',
      padding: '20px',
      maxWidth: '800px',
      margin: '0 auto'
    }}>
      <div style={{ marginBottom: '20px', display: 'flex', justifyContent: 'center' }}>
        <button 
          onClick={() => setAppMode('user')}
          style={{ 
            padding: '8px 16px', 
            marginRight: '10px',
            backgroundColor: appMode === 'user' ? '#000' : '#f0f0f0',
            color: appMode === 'user' ? '#fff' : '#000',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer'
          }}
        >
          Widget (User View)
        </button>
        <button 
          onClick={() => setAppMode('admin')}
          style={{ 
            padding: '8px 16px', 
            marginRight: '10px',
            backgroundColor: appMode === 'admin' ? '#E91E63' : '#f0f0f0',
            color: appMode === 'admin' ? '#fff' : '#000',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer'
          }}
        >
          Admin Panel (Config)
        </button>
        <button 
          onClick={() => setAppMode('minigame')}
          style={{ 
            padding: '8px 16px', 
            backgroundColor: appMode === 'minigame' ? '#2196F3' : '#f0f0f0',
            color: appMode === 'minigame' ? '#fff' : '#000',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer'
          }}
        >
          Minigame Test
        </button>
      </div>

      {appMode === 'user' && <DashboardWidget />}
      {appMode === 'admin' && <AdminPanel />}
      {appMode === 'minigame' && (
        <div style={{
          display: 'flex', flexDirection: 'column', alignItems: 'center', marginTop: '40px'
        }}>
          <h3 style={{ marginBottom: '16px', color: '#333' }}>Constrained Widget Testing Area</h3>
          <div style={{
            width: '320px', height: '160px', 
            border: '2px solid red', 
            borderRadius: '8px',
            position: 'relative',
            backgroundColor: '#fff',
            boxShadow: '0 4px 12px rgba(0,0,0,0.1)'
          }}>
            <FlappyBug onClose={() => setAppMode('user')} />
          </div>
          <p style={{ marginTop: '16px', fontSize: '13px', color: '#666' }}>
            This red box matches the approximate dimensions of the widget popover area.<br/>
            The game intrinsically scales to fit inside any container boundaries.
          </p>
        </div>
      )}
    </div>
  );
}

export default App;
