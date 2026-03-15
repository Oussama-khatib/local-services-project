import React from "react";

const TableComponent = ({ columns, data }) => {
  return (
    <div className="card overflow-x-auto">
      <table className="w-full text-left">
        <thead>
          <tr className="border-b">
            {columns.map((col, index) => (
              <th key={index} className="py-3 text-sm text-gray-500">
                {col}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {data.map((row, rowIndex) => (
            <tr key={rowIndex} className="border-b hover:bg-gray-50">
              {Object.values(row).map((cell, cellIndex) => (
                <td key={cellIndex} className="py-3 text-sm">
                  {cell}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TableComponent;
