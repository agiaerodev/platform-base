namespace Icomments.Config
{
    public class Permissions
    {    
        public static string GetPermissions()
        {
            return permissions;
        }

        private static string permissions = @"{
        'icomments.icomments': {
                'manage': 'icomments::icomments.manage',
                'index': 'icomments::icomments.list resource',
                'edit': 'icomments::icomments.edit resource',
                'create': 'icomments::icomments.create resource',
                'destroy': 'icomments::icomments.destroy resource',
                'restore': 'icomments::icomments.restore resource'
                }
//append
        }";
    }
}

