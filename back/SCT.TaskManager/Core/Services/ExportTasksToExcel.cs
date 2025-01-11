using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Core.Services
{
    public class ExportTasksToExcel
    {
        /// <summary>
        /// Генерация Excel-файла из массива задач.
        /// </summary>
        /// <param name="tasks">Список задач.</param>
        /// <returns>Поток MemoryStream с содержимым Excel-файла.</returns>
        public async Task<MemoryStream> ExportAsync(List<TaskDto> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                throw new ArgumentException("The tasks list is empty or null.");
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tasks");

                worksheet.Cells[1, 1].Value = "Название";
                worksheet.Cells[1, 2].Value = "Описание";
                worksheet.Cells[1, 3].Value = "Дата начала";
                worksheet.Cells[1, 4].Value = "Дата конца";

                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                for (int i = 0; i < tasks.Count; i++)
                {
                    var task = tasks[i];
                    worksheet.Cells[i + 2, 1].Value = task.Name;
                    worksheet.Cells[i + 2, 2].Value = task.Description;
                    worksheet.Cells[i + 2, 3].Value = task.StartDateTime.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 4].Value = task.EndDateTime.ToString("yyyy-MM-dd");
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                await Task.Run(() => package.SaveAs(stream));
            }

            // Устанавливаем позицию потока в начало
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Формирует список задач по ID проекта и экспортирует их в Excel.
        /// </summary>
        /// <param name="repository">Репозиторий задач.</param>
        /// <param name="projectId">ID проекта.</param>
        /// <returns>Поток MemoryStream с содержимым Excel-файла.</returns>
        public async Task<MemoryStream> ExportProjectTasksAsync(ITasksRepository repository, int projectId)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository), "TasksRepository cannot be null.");
            }

            var tasks = await repository.GetAllAsync(projectId);
            if (!tasks.Any())
            {
                throw new InvalidOperationException($"No tasks found for project with ID: {projectId}.");
            }

            return await ExportAsync(tasks);
        }
    }
}
