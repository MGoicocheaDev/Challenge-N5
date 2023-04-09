namespace web_api_lib_application.Logic.Dtos
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public DateTime FechaPermiso { get; set; }

        public int PermissionTypeId { get; set; }
        public string PermissionTypeName { get; set; }
    }
}
