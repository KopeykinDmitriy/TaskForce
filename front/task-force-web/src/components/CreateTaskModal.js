// src/components/CreateTaskModal.js

import React, { useState } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { createTask } from '../services/api';

const CreateTaskModal = ({ showModal, handleClose }) => {
  const [taskTitle, setTaskTitle] = useState('');
  const [taskDescription, setTaskDescription] = useState('');
  const [priority, setPriority] = useState('medium');

  const handleCreateTask = () => {
    // Здесь будет логика создания задачи, например, вызов API
    console.log('Task Created:', { taskTitle, taskDescription });
    createTask({ title: taskTitle, description: taskDescription, priority, status: "to_do" })
    
    // Очистка полей
    setTaskTitle('');
    setTaskDescription('');
    
    // Закрытие модального окна
    handleClose();
  };

  return (
    <Modal show={showModal} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Создать задачу</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form>
          <div className="mb-3">
            <label htmlFor="taskTitle" className="form-label">Название задачи</label>
            <input
              type="text"
              className="form-control"
              id="taskTitle"
              value={taskTitle}
              onChange={(e) => setTaskTitle(e.target.value)}
              required
            />
          </div>
          <div className="mb-3">
            <label htmlFor="taskDescription" className="form-label">Описание задачи</label>
            <textarea
              className="form-control"
              id="taskDescription"
              rows="3"
              value={taskDescription}
              onChange={(e) => setTaskDescription(e.target.value)}
              required
            />
            <label for="priority-select">Приоритет</label>
            <select class="form-select" id="priority-select" value={priority} onChange={(e) => setPriority(e.target.value)}>
              <option value="very high">Very High</option>
              <option value="high">High</option>
              <option value="medium">Medium</option>
              <option value="low">Low</option>
              <option value="lowest">Lowest</option>
            </select>
          </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          Закрыть
        </Button>
        <Button variant="primary" onClick={handleCreateTask}>
          Создать задачу
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default CreateTaskModal;
