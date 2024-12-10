import React, { useState } from 'react';
import ReactDOM from 'react-dom';
import Select from 'react-select';
import '../styles/AddUserModal.css';

const existingUsers = [
  { id: 3, name: 'Алексей' },
  { id: 4, name: 'Екатерина' },
  { id: 5, name: 'Сергей' },
];

const userOptions = existingUsers.map(user => ({ value: user.id, label: user.name }));

const AddUserModal = ({ onAdd, onClose }) => {
  const [selectedUser, setSelectedUser] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!selectedUser) return;

    const userToAdd = existingUsers.find(user => user.id === selectedUser.value);
    onAdd({ ...userToAdd, role: 'user', tags: [] });
  };

  return ReactDOM.createPortal(
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h3>Добавить пользователя</h3>
        <form onSubmit={handleSubmit}>
          <Select
            options={userOptions}
            value={selectedUser}
            onChange={setSelectedUser}
            placeholder="Выберите пользователя..."
          />
          <div className="modal-actions">
            <button type="submit">Добавить</button>
            <button type="button" onClick={onClose}>Отмена</button>
          </div>
        </form>
      </div>
    </div>,
    document.body
  );
};

export default AddUserModal;
