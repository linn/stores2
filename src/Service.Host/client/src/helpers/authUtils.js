import config from '../config';

const authority = config.cognitoHost;
const clientId = config.cognitoClientId;
const domainPrefix = config.cognitoDomainPrefix;
const origin = window.location.origin;

const redirectUri = origin + '/stores2/';

const logoutUri = origin + '/stores2/logged-out';

function getCognitoDomain(domainPrefix, authorityUri) {
    if (domainPrefix && authorityUri) {
        const regionMatch = authorityUri.match(/cognito-idp\.(.+)\.amazonaws\.com/);
        const region = regionMatch ? regionMatch[1] : '';
        return `https://${domainPrefix}.auth.${region}.amazoncognito.com`;
    }
    return '';
}

const cognitoDomain = getCognitoDomain(domainPrefix, authority);

export const oidcConfig = {
    authority,
    client_id: clientId,
    redirect_uri: redirectUri,
    response_type: 'code',
    scope: 'email openid profile',
    post_logout_redirect_uri: logoutUri,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    onSigninCallback: () => {
        const redirect = sessionStorage.getItem('auth:redirect');
        if (redirect) {
            window.location.href = redirect;
            sessionStorage.removeItem('auth:redirect');
        } else {
            window.location.href = `${origin}/stores2/`;
        }
    }
};

// hardcoded for now, could come from configs later
// todo get from configs!!
const entraLogoutUri = config.entraLogoutUri;
export const signOut = () => {
    if (!cognitoDomain) return;

    window.location.href = `${cognitoDomain}/logout?client_id=${clientId}&logout_uri=${encodeURIComponent(logoutUri)}`;
};

// need this extra logout step otherwise the user stays logged into entra id
// and can just reauthenticate immediately (note this will sign the user out of their microsoft account in their
// browser completely, so outlook, office 365, onedrive etc too)
export const signOutEntra = () => {
    window.location.href = `${entraLogoutUri}?post_logout_redirect_uri=${encodeURIComponent(logoutUri)}`;
};
