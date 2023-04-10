import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const ViewPermission = () => {

    const { permissionid } = useParams();

    useEffect(() => {
        axios.get(API_BASE_URL + '/' + permissionid).then((response) => {
            idchange(response.data.id);
            namechange(response.data.nombreEmpleado);
            lastnamechange(response.data.apellidoEmpleado);
            permissionTypeNamechange(response.data.permissionTypeName);
            datePermissionchange(response.data.fechaPermiso);
          });
    }, []);

    
    const[id,idchange]=useState("");
    const[firstName,namechange]=useState("");
    const[lastname,lastnamechange]=useState("");
    const[datepermission,datePermissionchange]=useState("");
    const[permissionTypeName,permissionTypeNamechange]=useState("");


    const navigate=useNavigate();
    const API_BASE_URL = "https://localhost:7121/api/Permisions"

    return(
        <div>
            <div className="row">
                <div className="offset-lg-3 col-lg-6">
                    <form className="container">
                        <div className="card" style={{"textAlign":"left"}}>
                            <div className="card-title">
                                <h2>View Permission</h2>
                            </div>
                            <div className="card-body">
                                <div className="row">
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Id</label>
                                            <input value={id} disabled="disabled" className="form-control"></input>
                                        </div>
                                    </div>
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Name</label>
                                            <input value={firstName} disabled="disabled" className="form-control"></input>
                                        </div>
                                    </div>
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Last Name</label>
                                            <input value={lastname} disabled="disabled" className="form-control"></input>
                                        </div>
                                    </div>
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Permission</label>
                                            <input value={permissionTypeName} disabled="disabled" className="form-control"></input>
                                        </div>
                                    </div>
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Date</label>
                                            <input value={datepermission} disabled="disabled" className="form-control"></input>
                                        </div>
                                    </div>

                                    <div className="col-lg-12">
                                        <div className="form-group">
                                           <Link to="/" className="btn btn-danger">Back</Link>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
export default ViewPermission