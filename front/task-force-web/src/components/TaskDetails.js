// src/components/TaskDetails.js

import React, { useState, useEffect } from 'react';
import { Card, Button, Form, Row, Col, Dropdown } from 'react-bootstrap';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchTaskById, updateTask } from '../services/api';

const TaskDetails = () => {
  const { taskId } = useParams();
  const [task, setTask] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [updatedTitle, setUpdatedTitle] = useState('');
  const [updatedDescription, setUpdatedDescription] = useState('');
  const [updatedStatus, setUpdatedStatus] = useState(''); // Новый статус задачи
  const navigate = useNavigate();

  useEffect(() => {
    const getTask = async () => {
      const data = await fetchTaskById(taskId);
      setTask(data);
      setUpdatedTitle(data.title);
      setUpdatedDescription(data.description);
      setUpdatedStatus(data.status); // Установить текущий статус
    };
    getTask();
  }, [taskId]);

  const handleUpdate = async () => {
    const updatedTask = {
      ...task,
      title: updatedTitle,
      description: updatedDescription,
      status: updatedStatus, // Сохранение нового статуса
    };
    await updateTask(taskId, updatedTask);
    setTask(updatedTask);
    setIsEditing(false);
  };

  const handleCancel = () => {
    setIsEditing(false);
    setUpdatedTitle(task.title);
    setUpdatedDescription(task.description);
    setUpdatedStatus(task.status); // Вернуть старый статус при отмене
  };

  if (!task) {
    return <div>Загрузка...</div>;
  }

  return (
    <div className="container mt-5">
      <Row>
        <Col md={{ span: 8, offset: 2 }}>
          <Card className="shadow-lg">
            <Card.Body>
              <Card.Title className="text-center">
                {isEditing ? (
                  <Form.Control 
                    type="text" 
                    value={updatedTitle} 
                    onChange={(e) => setUpdatedTitle(e.target.value)} 
                  />
                ) : (
                  <h2>{task.title}</h2>
                )}
              </Card.Title>
              <Card.Text>
                {isEditing ? (
                  <Form.Control 
                    as="textarea" 
                    rows={5} 
                    value={updatedDescription} 
                    onChange={(e) => setUpdatedDescription(e.target.value)} 
                  />
                ) : (
                  <p>{task.description}</p>
                )}
              </Card.Text>

              <div className="mb-4">
                <h5>Статус задачи:</h5>
                {isEditing ? (
                  <Form.Select
                    value={updatedStatus}
                    onChange={(e) => setUpdatedStatus(e.target.value)}
                  >
                    <option value="to_do">To Do</option>
                    <option value="in_progress">In Progress</option>
                    <option value="done">Done</option>
                  </Form.Select>
                ) : (
                  <p>{task.status}</p>
                )}
              </div>

              <div className="d-flex justify-content-between">
                {isEditing ? (
                  <>
                    <Button variant="primary" onClick={handleUpdate}>
                      Сохранить изменения
                    </Button>
                    <Button variant="secondary" onClick={handleCancel}>
                      Отменить
                    </Button>
                  </>
                ) : (
                  <Button variant="primary" onClick={() => setIsEditing(true)}>
                    Редактировать
                  </Button>
                )}

                <Button variant="secondary" onClick={() => navigate(-1)}>
                  Назад
                </Button>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default TaskDetails;
