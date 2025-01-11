import React, { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { fetchTaskById, updateTask, createTask, getAllTags, getUsers, predict } from '../services/api';
import '../styles/EditCreateTaskForm.css';

const EditCreateTaskForm = () => {
  const defaultTask = {
    name: "Task's title",
    tags: [],
    priority: 'Medium',
    endDateTime: new Date().toISOString().split('T')[0],
    status: 'TO_DO',
    description: '',
    projectId: 0,
    creatorName: '',
    executorName: ''
  };
  const navigate = useNavigate();

  const { taskId, projectId } = useParams();
  const isNew = taskId === undefined;

  const [task, setTask] = useState(defaultTask);
  const [allTags, setAllTags] = useState([]);
  const [users, setUsers] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      if (taskId !== undefined) {
        const data = await fetchTaskById(taskId);
        data.tags = data.tags.join(',');
        setTask(data);
      } else {
        const data = defaultTask;
        data.tags = data.tags.join(',');
        setTask(data);
      }
      const tags = await getAllTags();
      setAllTags(tags);

      const usersData = await getUsers();
      setUsers(usersData);
    };
    fetchData();
  }, [taskId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setTask({ ...task, [name]: value });
  };

  const handleSave = async () => {
    const updatedTask = {
      ...task,
      tags: task.tags ? task.tags.split(',') : [],
      id: task.id || 0,
      projectId: projectId
    };
    if (updatedTask.id === 0) await createTask(updatedTask);
    else await updateTask(updatedTask);

    if (isNew) navigate(`/${projectId}/`);
    else navigate(`/${projectId}/tasks/${task.id}`);
  };

  const getDate = (dateString) => {
    if (typeof dateString !== 'string' || dateString.includes('-')) return dateString;
    let dateParts = dateString.split('.');
    let date = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
    return date.toISOString().split('T')[0];
  };

  const handlePredictDate = async () => {
      const taskData = {
        task_name: task.name, 
        description: task.description, 
        tags: task.tags
      }
      const predictResult = await predict(taskData);
      const predictedDate = new Date();
      predictedDate.setDate(predictedDate.getDate() + predictResult.predicted_days);
      setTask({
        ...task,
        endDateTime: predictedDate.toISOString().split("T")[0],
      });
    
  };

  return (
    <div className="task-modal">
      <div className="form-content">
        <h3>{isNew ? 'Create New Task' : 'Edit Task'}</h3>
        <input
          type="text"
          name="name"
          value={task.name}
          onChange={handleChange}
          placeholder="Title"
          className="form-input"
        />
        <input
          type="text"
          name="tags"
          value={task.tags}
          onChange={handleChange}
          placeholder="Tags (comma-separated)"
          className="form-input"
          list="tags-list"
        />
        <datalist id="tags-list">
          {allTags.map((tag) => (
            <option key={tag} value={tag} />
          ))}
        </datalist>
        <select
          name="priority"
          value={task.priority}
          onChange={handleChange}
          className="form-input"
        >
          <option value="very high">Very High</option>
          <option value="high">High</option>
          <option value="medium">Medium</option>
          <option value="low">Low</option>
          <option value="very low">Very Low</option>
        </select>
        <textarea
          name="description"
          value={task.description}
          onChange={handleChange}
          placeholder="Description"
          className="form-input textarea"
        />
        <select
          name="executorName"
          value={task.executorName}
          onChange={handleChange}
          className="form-input"
        >
          <option value="">Select Executor</option>
          {users.map((user) => (
            <option key={user.id} value={user.name}>
              {user.name}
            </option>
          ))}
        </select>
        <div className="date-predict-container">
          <input
            type="date"
            name="endDateTime"
            value={getDate(task.endDateTime)}
            onChange={handleChange}
            className="form-input"
          />
          <button onClick={handlePredictDate} className="predict-btn">
            Predict
          </button>
        </div>
        <div className="actions">
          <Link
            className="link-without"
            to={isNew ? `/${projectId}/` : `/${projectId}/tasks/${task.id}`}
          >
            <button className="cancel-btn">Cancel</button>
          </Link>
          <button className="save-btn" onClick={handleSave}>
            {isNew ? 'Create' : 'Save'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditCreateTaskForm;
