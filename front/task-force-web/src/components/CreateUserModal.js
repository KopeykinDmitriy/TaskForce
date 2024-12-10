import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom';
import Select from 'react-select';
import '../styles/UserModal.css';

const existingTags = [
  'tag1', 'tag2', 'tag3', 'tag4', 'tag5', 'tag6', 'tag7', 'tag8', 'tag9', 'tag10',
  'tag11', 'tag12', 'tag13', 'tag14', 'tag15'
];

const tagOptions = existingTags.map(tag => ({ value: tag, label: tag }));

const CreateUserModal = ({ type, user, onSave, onClose }) => {
  const [name, setName] = useState(user?.name || '');
  const [role, setRole] = useState(user?.role || 'user');
  const [selectedTags, setSelectedTags] = useState(user?.tags?.map(tag => ({ value: tag, label: tag })) || []);
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  useEffect(() => {
    if (type === 'edit' && user) {
      setName(user.name);
      setRole(user.role);
      setSelectedTags(user.tags.map(tag => ({ value: tag, label: tag })));
    }
  }, [type, user]);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (type === 'create' && (!login || !password)) {
      alert('Логин и пароль обязательны для заполнения.');
      return;
    }

    const userData = {
      name,
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
          <input
            type="text"
            placeholder="Имя пользователя"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
          {type === 'create' && (
            <>
              <input
                type="text"
                placeholder="Логин"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
                required
              />
              <input
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
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