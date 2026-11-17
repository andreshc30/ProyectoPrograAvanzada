using ProyectoPA.EF;
using ProyectoPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;


namespace ProyectoPA.Services
{
    public class UsuarioService
    {
        private readonly byte[] Key = Convert.FromBase64String("sy+v61jJHH5tZTjth2g+PwKpfUiNXWA0ImlUWxikTKUedfPEC6zrkDSzsneTbfkTXJnaEchH7BcVBF4XkqIMDw==");

        #region Cifrar
        public string Cifrar(int userId, TimeSpan vigencia)
        {
            var payload = Encoding.UTF8.GetBytes($"{userId}|{DateTime.UtcNow.Add(vigencia).Ticks}");
            using (var aes = Aes.Create())
            using (var hmac = new HMACSHA256(Key.AsSpan(32).ToArray()))
            {
                aes.Key = Key.AsSpan(0, 32).ToArray();
                aes.GenerateIV();
                var cifrado = aes.CreateEncryptor().TransformFinalBlock(payload, 0, payload.Length);

                var datos = new byte[16 + cifrado.Length + 32];
                Buffer.BlockCopy(aes.IV, 0, datos, 0, 16);
                Buffer.BlockCopy(cifrado, 0, datos, 16, cifrado.Length);
                Buffer.BlockCopy(hmac.ComputeHash(datos, 0, 16 + cifrado.Length), 0, datos, 16 + cifrado.Length, 32);

                return Convert.ToBase64String(datos).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }
        }
        #endregion

        #region Descifrar
        public int? Descifrar(string token)
        {
            try
            {
                var b64 = token.Replace('-', '+').Replace('_', '/');
                var datos = Convert.FromBase64String(b64.PadRight(b64.Length + (4 - b64.Length % 4) % 4, '='));
                if (datos.Length < 16 + 16 + 32) return null;

                int largoMac = datos.Length - 32;
                using (var hmac = new HMACSHA256(Key.AsSpan(32).ToArray()))
                {
                    var macEsperado = hmac.ComputeHash(datos, 0, largoMac);
                    int diff = 0;
                    for (int i = 0; i < 32; i++) diff |= macEsperado[i] ^ datos[largoMac + i];
                    if (diff != 0) return null;
                }

                using (var aes = Aes.Create())
                {
                    aes.Key = Key.AsSpan(0, 32).ToArray();
                    aes.IV = datos.AsSpan(0, 16).ToArray();
                    var plano = aes.CreateDecryptor().TransformFinalBlock(datos, 16, largoMac - 16);
                    var partes = Encoding.UTF8.GetString(plano).Split('|');

                    if (DateTime.UtcNow.Ticks > long.Parse(partes[1])) return null;
                    return int.Parse(partes[0]);
                }
            }
            catch { return null; }
        }
        #endregion

        #region Enviar Correo
        public async Task EnviarCorreo(string para, string asunto, string html)
        {
            var cuentaCorreo = "casadelaplacaweb@gmail.com";
            var contrasenaCorreo = "aumb enag mgmh xttr";

            if (string.IsNullOrEmpty(contrasenaCorreo))
                return;

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(cuentaCorreo));
            mensaje.To.Add(MailboxAddress.Parse(para));
            mensaje.Subject = asunto;
            mensaje.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(cuentaCorreo, contrasenaCorreo);
                await smtp.SendAsync(mensaje);
                await smtp.DisconnectAsync(true);
            }
        }
        #endregion



        #region Mostrar Usuarios

        public List<UsuarioModel> ListarUsuarios()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.SP_LISTAR_USUARIOS()
                    .Select(u => new UsuarioModel
                    {
                        IdUsuario = u.ID_USUARIO,
                        Cedula = u.CEDULA,
                        Nombre = u.NOMBRE,
                        PrimerApellido = u.PRIMER_APELLIDO,
                        SegundoApellido = u.SEGUNDO_APELLIDO,
                        Correo = u.CORREO,
                        IdEstado = u.ID_ESTADO
                    }).ToList();
            }
        }

        #endregion


        #region Obtener Usuario

        public UsuarioModel ObtenerUsuario(int id)
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.SP_OBTENER_USUARIO(id)
                    .Select(u => new UsuarioModel
                    {
                        IdUsuario = u.ID_USUARIO,
                        Cedula = u.CEDULA,
                        Nombre = u.NOMBRE,
                        PrimerApellido = u.PRIMER_APELLIDO,
                        SegundoApellido = u.SEGUNDO_APELLIDO,
                        PerfilUrl = u.PERFIL_URL,
                        Correo = u.CORREO,
                        IdEstado = u.ID_ESTADO,
                        IdDireccion = u.ID_DIRECCION ?? 0
                    }).FirstOrDefault();
            }
        }

        #endregion


        #region Editar Usuario

        public int EditarUsuario(UsuarioModel usuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                var resultado = db.SP_EDITAR_USUARIO(
                    usuario.IdUsuario,
                    usuario.Cedula,
                    usuario.Nombre,
                    usuario.PrimerApellido,
                    usuario.SegundoApellido,
                    usuario.PerfilUrl,
                    usuario.Correo,
                    usuario.IdEstado,
                    usuario.IdDireccion
                ).FirstOrDefault();

                return resultado ?? 0;
            }
        }

        #endregion


        #region Obtener Estados

        public List<FIDE_ESTADO_TB> ObtenerEstados()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.FIDE_ESTADO_TB
                    .AsNoTracking()
                    .ToList();
            }
        }

        #endregion


        #region Obtener Direcciones

        public List<FIDE_DIRECCION_TB> ObtenerDirecciones()
        {
            using (var db = new FIDE_DBEntities())
            {
                return db.FIDE_DIRECCION_TB
                    .AsNoTracking()
                    .ToList();
            }
        }

        #endregion


        #region Administrar Roles

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

        public void AsignarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_ASIGNAR_ROL_USUARIO(idUsuario, idRol);
            }
        }

        public void QuitarRol(int idUsuario, int idRol)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_QUITAR_ROL_USUARIO(idUsuario, idRol);
            }
        }

        #endregion


        #region Desactivar Usuario

        public void DesactivarUsuario(int idUsuario)
        {
            using (var db = new FIDE_DBEntities())
            {
                db.SP_DESACTIVAR_USUARIO(idUsuario);
            }
        }

        #endregion
    }
}