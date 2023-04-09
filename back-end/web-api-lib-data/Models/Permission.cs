namespace web_api_lib_data.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public DateTime FechaPermiso { get; set; }

        public virtual PermissionType PermissionTypes { get; set; }
    }
}
