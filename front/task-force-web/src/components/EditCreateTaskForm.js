import React, { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { fetchTaskById, updateTask, createTask } from '../services/api';
import '../styles/EditCreateTaskForm.css'

const EditCreateTaskForm = () => {
  const defaultTask = {
    title: "Task's title",
    tags: [],
    priority: 'Medium',
    creator: '',
    performer: '',
    date: new Date(),
    status: '-------',
    description: `-----`,
  };
  const navigate = useNavigate();

  const { taskId } = useParams();
  const isNew = taskId === 0;
  console.log(taskId);
  const [task, setTask] = useState(defaultTask);

  useEffect(() => {
    const getTask = async () => {
      const data = await fetchTaskById(taskId)
      data.tags = data.tags.join(',');
      setTask(data)
    }
    getTask()
  }, [taskId])

  const handleChange = (e) => {
    const { name, value } = e.target;
    setTask({ ...task, [name]: value });
  };

  const handleSave = async () => {
    const updatedTask = {
      ...task,
      tags: task.tags.split(','), // Преобразуем строки в массив
      id: task.id || 0
    };
    if (updatedTask.id === 0)
      await createTask(updatedTask);
    else
      await updateTask(updatedTask.id, updatedTask);

    if (isNew) 
      navigate('/');
    else
      navigate(`/tasks/${task.id}`);
    
  };

  const getDate = (dateString) => {
    if (typeof(dateString) != "string" || dateString.includes('-'))
      return dateString;
    console.log(typeof(dateString));
    console.log(dateString);
    let dateParts = dateString.split('.');
    let date = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
    console.log(date);
    return date.toISOString().split('T')[0];
  } 

  return (
    <div className="task-modal">
      <div className="form-content">
        <h3>{isNew ? 'Create New Task' : 'Edit Task'}</h3>
        <input
          type="text"
          name="title"
          value={task.title}
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
        />
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
          <option value="very Low">Very Low</option>
        </select>
        <textarea
          name="description"
          value={task.description}
          onChange={handleChange}
          placeholder="Description"
          className="form-input textarea"
        />
        <input
          type="text"
          name="author"
          value={task.author}
          onChange={handleChange}
          placeholder="Author"
          className="form-input"
        />
        <input
          type="date"
          name="date"
          value={getDate(task.date)}
          onChange={handleChange}
          className="form-input"
        />
        <div className="actions">
          <Link class="link-without" to={isNew ? `/` : `/tasks/${task.id}` }>
            <button className="cancel-btn">
              Cancel
            </button>
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
