import React, { createContext, useContext, useState, ReactNode } from 'react';
import { Notification } from '@mantine/core';

interface NotificatieBericht {
  bericht: string;
  type: 'error' | 'success' | 'info';
  assertive: boolean;
  id: number;
}

interface NotificatieContextType {
  addNotificatie: (bericht: string, type?: 'error' | 'success' | 'info', assertive?: boolean) => void;
}

const NotificatieContext = createContext<NotificatieContextType | undefined>(undefined);

export function useNotificaties() {
  const context = useContext(NotificatieContext);

  if (!context) {
    throw new Error('useNotificaties kan niet buiten een NotificatieContext gebruikt worden');
  }

  return context;
}

interface NotificatieProviderProps {
  children: ReactNode;
}

export function NotificatieProvider({ children }: NotificatieProviderProps) {
  const [notificaties, setNotificaties] = useState<NotificatieBericht[]>([]);

  const addNotificatie = (bericht: string, type: 'error' | 'success' | 'info' = 'info', assertive: boolean = false) => {
    const newNotificatie = { bericht, type, assertive, id: Date.now() };

    setNotificaties([newNotificatie]);

    setTimeout(() => {
      setNotificaties((prev) => prev.filter((notif) => notif.id !== newNotificatie.id));
    }, 5000);
  };

  return (
    <NotificatieContext.Provider value={{ addNotificatie }}>
      <div
        role="region"
        style={{ position: 'fixed', top: 10, left: '50%', transform: 'translateX(-50%)', zIndex: 1000 }}
      >
        {notificaties.map((n) => (
          <Notification
            key={n.id}
            color={n.type === 'error' ? 'red' : n.type === 'success' ? 'green' : 'blue'}
            onClose={() => setNotificaties((prev) => prev.filter((notif) => notif.id !== n.id))}
            aria-live={n.assertive ? 'assertive' : 'polite'}
            style={{ marginBottom: 10 }}
          >
            {n.bericht}
          </Notification>
        ))}
      </div>

      {children}
    </NotificatieContext.Provider>
  );
}
