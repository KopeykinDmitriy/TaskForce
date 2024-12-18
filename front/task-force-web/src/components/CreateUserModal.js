import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import Select from 'react-select';
import '../styles/UserModal.css';
import { getAllTags } from '../services/api';

const CreateUserModal = ({ type, user, onSave, onClose }) => {
  const [role, setRole] = useState(user?.role || 'user');
  const [selectedTags, setSelectedTags] = useState(user?.tags?.map(tag => ({ value: tag, label: tag })) || []);
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [tagOptions, setTagOptions] = useState([]);

  useEffect(() => {
    if (type === 'edit' && user) {
      setRole(user.role);
      setSelectedTags(user.tags.map(tag => ({ value: tag, label: tag })));
    }

    const getTags = async () => {
      const tags = await getAllTags();
      const tagOptions = tags.map(tag => ({ value: tag, label: tag }));
      setTagOptions(tagOptions);
    }
    getTags();
  }, [type, user]);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (type === 'create' && (!login || !password)) {
      alert('Логин и пароль обязательны для заполнения.');
      return;
    }

    const userData = {
      role,
      tags: selectedTags.map(tag => tag.value),
    };

    if (type === 'create') {
      userData.login = login;
      userData.password = password;
    }

    onSave(userData);
  };

  return ReactDOM.createPortal(
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h3>{type === 'edit' ? 'Редактировать пользователя' : 'Создать пользователя'}</h3>
        <form onSubmit={handleSubmit}>
          {type === 'create' && (
            <>
              <input
                type="text"
                placeholder="Логин"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
                autoComplete="off"
                required
              />
              <input
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="new-password"
                required
              />
            </>
          )}
          <select value={role} onChange={(e) => setRole(e.target.value)}>
            <option value="user">User</option>
            <option value="admin">Admin</option>
          </select>

          <label>Выберите тэги:</label>
          <Select
            options={tagOptions}
            isMulti
            value={selectedTags}
            onChange={setSelectedTags}
            placeholder="Выберите тэги..."
          />

          <div className="modal-actions">
            <button type="submit">Сохранить</button>
            <button type="button" onClick={onClose}>Отмена</button>
          </div>
        </form>
      </div>
    </div>,
    document.body
  );
};

export default CreateUserModal;