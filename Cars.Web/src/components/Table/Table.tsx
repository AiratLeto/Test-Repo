import React from 'react';
import './Table.css';

/** Интерфейс для колонок таблицы */
interface TableColumn {
  header: string;
  key: string;
  customCell?: (index: number) => React.ReactNode
}

/** Интерфейс для данных в таблице */
interface TableData {
  [key: string]: any;
}

/** Интерфейс для пропсов компонента */
interface CustomTableProps {
  currentPage: number,
  onChangePage: (pageNumber: number) => void,
  totalCount: number,
  data: TableData[];
  columns: TableColumn[];
}

/** Компонент таблицы */
const Table: React.FC<CustomTableProps> = ({
  currentPage = 1,
  onChangePage,
  totalCount,
  data,
  columns,
}) => {
  return (
    <div>
      <table className="custom-table">
        <thead>
          <tr>
            {columns.map((column, index) => (
              <th key={index}>{column.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.length
            ? data.map((row, rowIndex) => (
              <tr key={rowIndex}>
                {columns.map((column, colIndex) => (
                  <td key={colIndex}>{column.customCell ? column.customCell(rowIndex) : row[column.key]}</td>
                ))}
              </tr>
            ))
            : <tr>
              <td
                style={{ textAlign: 'center' }}
                colSpan={columns.length}
              >Нет элементов
              </td>
            </tr>}
        </tbody>
      </table>
      {data.length > 0 && <div className="pagination">
        <button
          onClick={() => onChangePage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          Предыдущая страница
        </button>
        <span>{`Страница ${currentPage}`}</span>
        <button
          onClick={() => onChangePage(currentPage + 1)}
          disabled={Math.ceil(totalCount / data.length) == currentPage}
        >
          Следующая страница
        </button>
      </div>}
    </div>
  );
}

export default Table;