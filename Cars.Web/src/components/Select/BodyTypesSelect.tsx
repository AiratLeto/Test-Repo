import React, { useEffect, useMemo, useState } from 'react';
import { DictionaryItem } from '../../types/Dictionary';
import BodyTypeService from '../../services/BodyTypeService';
import './Select.css';

interface SelectProps {
  id?: string;
  onChange: (value: string) => void;
}

/** Селект справочника типов кузова */
const BodyTypesSelect: React.FC<SelectProps> = ({ id, onChange }) => {
  const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    onChange(event.target.value);
  };

  const [options, setOptions] = useState<DictionaryItem[]>();

  useEffect(() => {
    BodyTypeService.GetListAsync({
      pageNumber: 1,
      pageSize: 30
    })
      .then(response => setOptions(response.entities))
      .catch((e) => console.warn('Ошибка при получении данных', e));
  }, []);

  const localValue = useMemo(() => options?.find(x => x.id == id), [options]);

  return (
    <div className="select-container">
      <label>Тип кузова</label>
      <select value={localValue?.name} onChange={handleChange}>
        <option value="">Выберите</option>
        {options?.length
          ? options?.map((option) => (
            <option key={option.id} value={option.id}>
              {option.name}
            </option>
          ))
          : <option>Ничего не найдено</option>}
      </select>
    </div>
  );
};

export default BodyTypesSelect;