export const getCsrfToken = () => {
    const csrfToken = document.cookie
        .split('; ')
        .find(row => row.startsWith('csrfToken='))?.split('=')[1];

    if (!csrfToken) {
        console.error('CSRF token not found');
        return null; // Return null if token not found
    }

    return csrfToken; // Return the CSRF token
};
