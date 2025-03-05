namespace BackEnd_Server.DTOs
{
    public class TaskCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int State { get; set; }
        public int Order { get; set; }  // Puedes asignar un valor por defecto si lo deseas
        
        // Si no necesitas ChangeDetails en la creación, puedes omitirlo o dejarlo opcional.
        
        // Si el Sprint no es obligatorio al crear la tarea, lo dejamos como opcional (por Id)
        public int? SprintId { get; set; }
        
        // Aquí se envía únicamente el Id del ProductBacklog, lo que permitirá
        // que el backend lo recupere y lo asocie correctamente.
        public int ProductBacklogId { get; set; }
        
        // Opcional, si necesitas enviar datos del WeeklyScrums, o lo puedes omitir.
        
        // Enviar sólo el Id del Developer asignado
        public int? DeveloperId { get; set; }
    }

    // DTO para ChangeDetails (ajusta según tus necesidades)
}
