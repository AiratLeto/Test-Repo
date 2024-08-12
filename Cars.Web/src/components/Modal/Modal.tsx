import React from 'react';
import './Modal.css';

interface ModalProps {
  onClose: () => void;
  children: React.ReactNode
}

/** Модальное окно */
const Modal: React.FC<ModalProps> = ({ onClose, children }) => {
  const closeModal = () => {
    onClose();
  };

  return (
    <div className="modal" style={{ display: 'block' }}>
      <div className="modal-content">
        <button className="close" onClick={closeModal}>
          Закрыть
        </button>
        {children}
      </div>
    </div>
  );
};

export default Modal;