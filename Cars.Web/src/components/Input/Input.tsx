import React from 'react';
import './Input.css';

interface InputProps {
  label: string;
  value?: string | number;
  onChange: (value: string | number) => void;
  type?: 'number' | 'text',
  error?: string,
  min?: number,
  max?: number,
}

/** Базовый инпут для всей системы */
const Input: React.FC<InputProps> = ({ label, value, onChange, type = 'text', error, min, max }) => {
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  return (
    <div className="input-container">
      <label>{label}</label>
      <input
        type={type}
        value={value}
        onChange={handleChange}
        min={min}
        max={max}
        className='input-field'
      />
      {error && <div>{error}</div>}
    </div>
  );
};

export default Input;