import { useEffect } from 'react';
import './Notification.css';

interface NotificationProps {
  onClose: () => void;
  message: string
}

/** Боковое уведомление */
const Notification: React.FC<NotificationProps> = ({ message, onClose }) => {
  useEffect(() => {
    const timeout = setTimeout(() => {
      onClose();
    }, 3000); // Закрыть уведомление через 3 секунды (или другое подходящее время)

    return () => clearTimeout(timeout);
  }, [onClose]);

  return (
    <div className="notification">
      {message}
      <button onClick={onClose}>Закрыть</button>
    </div>
  );
};

export default Notification;