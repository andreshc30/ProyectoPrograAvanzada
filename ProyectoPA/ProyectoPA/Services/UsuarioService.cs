
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using ProyectoPA.EF;
using ProyectoPA.Models;

namespace ProyectoPA.Services
{
    public class UsuarioService
    {
        // MOSTRAR USUARIOS
        public List<UsuarioModel> ListarUsuarios()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database
                    .SqlQuery<UsuarioModel>(
                        "EXEC dbo.SP_LISTAR_USUARIOS")
                    .ToList();
            }
        }

        // OBTENER USUARIO POR ID
        public UsuarioModel ObtenerUsuario(int id)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database
                    .SqlQuery<UsuarioModel>(
                        "EXEC dbo.SP_OBTENER_USUARIO @ID_USUARIO",
                        new SqlParameter("@ID_USUARIO", id))
                    .FirstOrDefault();
            }
        }

        // CREAR USUARIO
        public int CrearUsuario(
            string cedula,
            string nombre,
            string primerApellido,
            string segundoApellido,
            string perfilUrl,
            string correo,
            int idEstado,
            string contrasenaHash,
            int idDireccion)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database.SqlQuery<int>(
                    @"EXEC dbo.SP_CREAR_USUARIO
                    @CEDULA,
                    @NOMBRE,
                    @PRIMER_APELLIDO,
                    @SEGUNDO_APELLIDO,
                    @PERFIL_URL,
                    @CORREO,
                    @ID_ESTADO,
                    @CONTRASENA,
                    @ID_DIRECCION",

                    new SqlParameter("@CEDULA", cedula),
                    new SqlParameter("@NOMBRE", nombre),
                    new SqlParameter("@PRIMER_APELLIDO", primerApellido),
                    new SqlParameter("@SEGUNDO_APELLIDO",
                        (object)segundoApellido ?? DBNull.Value),
                    new SqlParameter("@PERFIL_URL",
                        (object)perfilUrl ?? DBNull.Value),
                    new SqlParameter("@CORREO", correo),
                    new SqlParameter("@ID_ESTADO", idEstado),
                    new SqlParameter("@CONTRASENA", contrasenaHash),
                    new SqlParameter("@ID_DIRECCION", idDireccion)
                ).Single();
            }
        }

        // EDITAR USUARIO
        public int EditarUsuario(UsuarioModel usuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database.SqlQuery<int>(
                    @"EXEC dbo.SP_EDITAR_USUARIO
                    @ID_USUARIO,
                    @CEDULA,
                    @NOMBRE,
                    @PRIMER_APELLIDO,
                    @SEGUNDO_APELLIDO,
                    @PERFIL_URL,
                    @CORREO,
                    @ID_ESTADO,
                    @ID_DIRECCION",

                    new SqlParameter("@ID_USUARIO",
                        usuario.ID_USUARIO),
                    new SqlParameter("@CEDULA",
                        usuario.CEDULA),
                    new SqlParameter("@NOMBRE",
                        usuario.NOMBRE),
                    new SqlParameter("@PRIMER_APELLIDO",
                        usuario.PRIMER_APELLIDO),
                    new SqlParameter("@SEGUNDO_APELLIDO",
                        (object)usuario.SEGUNDO_APELLIDO
                        ?? DBNull.Value),
                    new SqlParameter("@PERFIL_URL",
                        (object)usuario.PERFIL_URL
                        ?? DBNull.Value),
                    new SqlParameter("@CORREO",
                        usuario.CORREO),
                    new SqlParameter("@ID_ESTADO",
                        usuario.ID_ESTADO.Value),
                    new SqlParameter("@ID_DIRECCION",
                        usuario.ID_DIRECCION.Value)
                ).Single();
            }
        }

        // OBTENER ESTADOS
        public List<FIDE_ESTADO_TB> ObtenerEstados()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.FIDE_ESTADO_TB
                    .AsNoTracking()
                    .ToList();
            }
        }

        // OBTENER DIRECCIONES
        public List<FIDE_DIRECCION_TB> ObtenerDirecciones()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.FIDE_DIRECCION_TB
                    .AsNoTracking()
                    .ToList();
            }
        }

        // LISTAR ROLES DISPONIBLES
        public List<FIDE_ROL_TB> ListarRoles()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database
                    .SqlQuery<FIDE_ROL_TB>(
                        "EXEC dbo.SP_LISTAR_ROLES")
                    .ToList();
            }
        }

        // CONSULTAR ROLES DE UN USUARIO
        public List<RolUsuarioModel> ObtenerRolesUsuario(int idUsuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.Database
                    .SqlQuery<RolUsuarioModel>(
                        "EXEC dbo.SP_ROLES_USUARIO @ID_USUARIO",
                        new SqlParameter("@ID_USUARIO", idUsuario))
                    .ToList();
            }
        }

        // ASIGNAR ROL
        public void AsignarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC dbo.SP_ASIGNAR_ROL_USUARIO @ID_USUARIO, @ID_ROL",
                    new SqlParameter("@ID_USUARIO", idUsuario),
                    new SqlParameter("@ID_ROL", idRol));
            }
        }

        // QUITAR ROL
        public void QuitarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC dbo.SP_QUITAR_ROL_USUARIO @ID_USUARIO, @ID_ROL",
                    new SqlParameter("@ID_USUARIO", idUsuario),
                    new SqlParameter("@ID_ROL", idRol));
            }
        }

        // DESACTIVAR USUARIO
        public void DesactivarUsuario(int idUsuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC dbo.SP_DESACTIVAR_USUARIO @ID_USUARIO",
                    new SqlParameter("@ID_USUARIO", idUsuario));
            }
        }
    }
}
