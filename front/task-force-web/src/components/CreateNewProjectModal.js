import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const CreateNewProjectModal = () => {
    const [username, setProjectName] = useState('');

    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        // Создание проекта.
        navigate('/')
      };

    return (
    <div class="base-container">
        <div class="base-box">
            <h1 class="base-title">Create new project</h1>
            <form class="base-form" onSubmit={handleSubmit}>
                <div class="input-group">
                    <label>
                        <i class="fa fa-user"></i>
                    </label>
                    <input type="text" id="projectName" name="projectName" placeholder="Project Name" required value={username} onChange={(e) => setProjectName(e.target.value)} />
                </div>
                <button type='submit' class="btn-submit">Create</button>
            </form>
        </div>
    </div>
    );
};

export default CreateNewProjectModal;