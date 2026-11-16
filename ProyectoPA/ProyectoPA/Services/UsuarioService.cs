
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                return db.SP_LISTAR_USUARIOS()
                    .Select(u => new UsuarioModel
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        CEDULA = u.CEDULA,
                        NOMBRE = u.NOMBRE,
                        PRIMER_APELLIDO = u.PRIMER_APELLIDO,
                        SEGUNDO_APELLIDO = u.SEGUNDO_APELLIDO,
                        CORREO = u.CORREO,
                        ID_ESTADO = u.ID_ESTADO
                    }).ToList();
            }
        }

        // OBTENER USUARIO POR ID
        public UsuarioModel ObtenerUsuario(int id)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.SP_OBTENER_USUARIO(id)
                    .Select(u => new UsuarioModel
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        CEDULA = u.CEDULA,
                        NOMBRE = u.NOMBRE,
                        PRIMER_APELLIDO = u.PRIMER_APELLIDO,
                        SEGUNDO_APELLIDO = u.SEGUNDO_APELLIDO,
                        PERFIL_URL = u.PERFIL_URL,
                        CORREO = u.CORREO,
                        ID_ESTADO = u.ID_ESTADO,
                        ID_DIRECCION = u.ID_DIRECCION
                    }).FirstOrDefault();
            }
        }

        // EDITAR USUARIO
        public int EditarUsuario(UsuarioModel usuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                var resultado = db.SP_EDITAR_USUARIO(
                    usuario.ID_USUARIO,
                    usuario.CEDULA,
                    usuario.NOMBRE,
                    usuario.PRIMER_APELLIDO,
                    usuario.SEGUNDO_APELLIDO,
                    usuario.PERFIL_URL,
                    usuario.CORREO,
                    usuario.ID_ESTADO,
                    usuario.ID_DIRECCION
                ).FirstOrDefault();

                return resultado ?? 0;
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

        // LISTAR ROLES
        public List<FIDE_ROL_TB> ListarRoles()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.SP_LISTAR_ROLES()
                    .Select(r => new FIDE_ROL_TB
                    {
                        ID_ROL = r.ID_ROL,
                        NOMBRE = r.NOMBRE,
                        ID_ESTADO = r.ID_ESTADO
                    }).ToList();
            }
        }

        // OBTENER ROLES DE UN USUARIO
        public List<RolUsuarioModel> ObtenerRolesUsuario(int idUsuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.SP_ROLES_USUARIO(idUsuario)
                    .Select(r => new RolUsuarioModel
                    {
                        ID_USUARIO = r.ID_USUARIO,
                        ID_ROL = r.ID_ROL,
                        ROL = r.ROL
                    }).ToList();
            }
        }

        // ASIGNAR ROL
        public void AsignarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_ASIGNAR_ROL_USUARIO(idUsuario, idRol);
            }
        }

        // QUITAR ROL
        public void QuitarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_QUITAR_ROL_USUARIO(idUsuario, idRol);
            }
        }

        // DESACTIVAR USUARIO
        public void DesactivarUsuario(int idUsuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_DESACTIVAR_USUARIO(idUsuario);
            }
        }
    }
}
