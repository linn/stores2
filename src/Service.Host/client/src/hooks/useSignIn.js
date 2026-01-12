import { useEffect, useState } from 'react';
import { useAuth, hasAuthParams } from 'react-oidc-context';

function useSignIn({ disabled = false } = {}) {
    const auth = useAuth();
    const [hasTriedSignin, setHasTriedSignin] = useState(false);

    useEffect(() => {
        // don't try to sign in if disabled (e.g., on logged-out route)
        if (disabled) {
            return;
        }

        if (
            !hasAuthParams() &&
            !auth.isAuthenticated &&
            !auth.activeNavigator &&
            !auth.isLoading &&
            !hasTriedSignin
        ) {
            auth.signinRedirect();
            sessionStorage.setItem('auth:redirect', window.location.pathname);
            setHasTriedSignin(true);
        }
    }, [auth, hasTriedSignin, disabled]);
}

export default useSignIn;
