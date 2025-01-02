import { AuthorizedUser, AuthorizeView } from "../../components/Authenticatie/AuthorizeView.tsx";
import { LogoutLink } from "../../components/Authenticatie/LogoutLink.tsx";

export function Test() {
    return (
        <AuthorizeView>
            <span><LogoutLink>Logout <AuthorizedUser value="email"/></LogoutLink></span>
        </AuthorizeView>
    )
}