import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import SelectPermission from "./select/selectPermission";

const EditPermission = () => {

    const { permissionid } = useParams();

    useEffect(() => {
        axios.get(API_BASE_URL + '/' + permissionid).then((response) => {
            idchange(response.data.id);
            namechange(response.data.nombreEmpleado);
            lastnamechange(response.data.apellidoEmpleado);
            permissionidtypechange(response.data.permissionTypeId);
          });
    }, []);

    
    const[id,idchange]=useState("");
    const[firstName,namechange]=useState("");
    const[lastname,lastnamechange]=useState("");
    const[permissionTypeId,permissionidtypechange]=useState(1);


    const navigate=useNavigate();
    const API_BASE_URL = "http://localhost:8098/api/Permisions"

    const handlesubmit=(e)=>{
        e.preventDefault();
        const permissionData={id,firstName,lastname,permissionTypeId};
    
        axios.put(API_BASE_URL, permissionData, {
            headers: {
              'content-type': 'application/json'
            }})
        .then((response) => {
            alert('Saved successfully.')
            navigate('/');
        }); 
  
      }

    const getPermissionSelected =(select) => {
        console.log("valor seleccionado: " + select);
        permissionidtypechange(select);
    }

    return(
        <div>
            <div className="row">
                <div className="offset-lg-3 col-lg-6">
                    <form className="container" onSubmit={handlesubmit}>
                        <div className="card" style={{"textAlign":"left"}}>
                            <div className="card-title">
                                <h2>Update Permission</h2>
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
                                            <input required value={firstName} onChange={e=>namechange(e.target.value)} className="form-control"></input>
                                        </div>
                                    </div>
                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Last Name</label>
                                            <input required value={lastname}  onChange={e=>lastnamechange(e.target.value)} className="form-control"></input>
                                        </div>
                                    </div>

                                    <div className="col-lg-12">
                                        <div className="form-group">
                                            <label>Permission</label>
                                            <SelectPermission getPermissionSelected={getPermissionSelected}  valuePermissionSelected={permissionTypeId}/>
                                        </div>
                                    </div>

                                    <div className="col-lg-12">
                                        <div className="form-group">
                                           <button className="btn btn-success" type="submit">Save</button>
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
export default EditPermission