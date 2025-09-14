using System.Collections.Generic;
using BusinessLogic.Models;

namespace Services.Hateoas
{
    public static class ResourceLinker
    {
        public static void AddLinksToUser(UserDto user)
        {
            user.Links.Clear();
            user.Links.AddRange(GetUserLinks(user.IdUsuario));
        }

        public static void AddLinksToPersona(PersonaDto persona)
        {
            persona.Links.Clear();
            persona.Links.AddRange(GetPersonaLinks(persona.IdPersona));
        }

        public static List<LinkSpec> GetUserLinks(int id)
        {
            return new List<LinkSpec>
            {
                new LinkSpec("GetUserById", new { id }, "self", "GET"),
                new LinkSpec("DeleteUser", new { id }, "delete_user", "DELETE"),
                new LinkSpec("UpdateUser", new { id }, "update_user", "PUT"),
                new LinkSpec("PatchUser", new { id }, "patch_user", "PATCH")
            };
        }

        public static List<LinkSpec> GetPersonaLinks(int id)
        {
            return new List<LinkSpec>
            {
                new LinkSpec("GetPersonaById", new { id }, "self", "GET"),
                new LinkSpec("DeletePersona", new { id }, "delete_persona", "DELETE"),
                new LinkSpec("UpdatePersona", new { id }, "update_persona", "PUT"),
                new LinkSpec("PatchPersona", new { id }, "patch_persona", "PATCH")
            };
        }
    }
}
