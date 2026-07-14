// Android compile-time shims for System.Web namespaces.
//
// A handful of original Duck Game files do `using System.Web.UI.WebControls;`,
// `using System.Web.Profile;`, `using System.Web.UI;`, `using System.Web.Configuration;`
// and `using System.Web.UI.WebControls.WebParts;`. On .NET they pull in ASP.NET which
// is not available on net8.0-android. The original code never actually *uses* any
// System.Web type at runtime on these paths (the imports are vestigial), but the
// compiler still needs the namespaces to resolve. These empty namespaces satisfy that
// without modifying the game source.

namespace System.Web.UI.WebControls { }
namespace System.Web.UI.WebControls.WebParts { }
namespace System.Web.UI { }
namespace System.Web.Profile { }
namespace System.Web.Configuration { }
