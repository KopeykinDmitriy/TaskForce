import React, { useState } from 'react';
import '../styles/UserManagement.css';
import CreateUserModal from './CreateUserModal';
import AddUserModal from './AddUserModal';

const initialUsers = [
    { id: 1, name: 'Иван', role: 'user', tags: ['tag1', 'tag2'] },
    { id: 2, name: 'Мария', role: 'admin', tags: ['tag3'] },
    { id: 3, name: 'Алексей', role: 'user', tags: ['tag1', 'tag4'] },
  ];
  
  const UserManagementPage = () => {
    const [users, setUsers] = useState(initialUsers);
    const [modalType, setModalType] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);
    const [isAddUserModalOpen, setIsAddUserModalOpen] = useState(false);
  
    const openModal = (type, user = null) => {
      setModalType(type);
      setCurrentUser(user);
    };
  
    const closeModal = () => {
      setModalType(null);
      setCurrentUser(null);
    };

    const openAddUserModal = () => {
      setIsAddUserModalOpen(true);
    };
  
    const handleSaveUser = (userData) => {
      if (currentUser) {
        // При редактировании обновляем данные пользователя
        setUsers(users.map(u => (u.id === currentUser.id ? { ...u, ...userData } : u)));
      } else {
        // При создании нового пользователя добавляем его в список
        setUsers([...users, { ...userData, id: Date.now() }]);
      }
  
      closeModal();
  
      // Здесь можно отправить данные на бэкенд для создания/обновления пользователя
      // например: fetch('/api/users', { method: 'POST', body: JSON.stringify(userData) });
    };

    const handleAddExistingUser = (user) => {
      setUsers([...users, user]);
      setIsAddUserModalOpen(false);
    };
  
    return (
      <div className="content">
        <div className="user-management">
          <h2 className="users-h2">Управление пользователями</h2>
          <div className="buttons">
            <button className="users-button" onClick={() => openModal('create')}>Создать пользователя</button>
            <button className="users-button" onClick={openAddUserModal}>Добавить существующего пользователя</button>
          </div>
          <table>
            <thead>
              <tr>
                <th>Имя</th>
                <th>Роль</th>
                <th>Разрешенные тэги</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {users.map(user => (
                <tr key={user.id}>
                  <td>{user.name}</td>
                  <td>{user.role}</td>
                  <td>{user.tags.join(', ')}</td>
                  <td>
                    <button className='users-button' onClick={() => openModal('edit', user)}>Редактировать</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
  
        {modalType && (
          <CreateUserModal
            type={modalType}
            user={currentUser}
            onSave={handleSaveUser}
            onClose={closeModal}
          />
        )}
        {isAddUserModalOpen && (
        <AddUserModal
          onAdd={handleAddExistingUser}
          onClose={() => setIsAddUserModalOpen(false)}
        />
      )}
      </div>
    );
  };
  
  export default UserManagementPage;