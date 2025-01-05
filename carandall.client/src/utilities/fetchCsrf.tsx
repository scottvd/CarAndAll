export const fetchCsrf = async (url: string, options: RequestInit = {}) => {
  const csrfToken = document.cookie
      .split('; ')
      .find(row => row.startsWith('csrfToken='))?.split('=')[1];

  if (!csrfToken) {
      console.error('CSRF token not found');
      return;
  }

  const headers = {
      ...options.headers,
      'X-CSRF-Token': csrfToken,
  };

  const response = await fetch(url, {
      ...options,
      method: options.method || 'GET',
      headers: headers,
      credentials: 'include',
  });

  if (!response.ok) {
      const errorText = await response.text();
      console.error('API request failed', errorText);
      throw new Error(errorText);
  }

  try {
      return await response.json();
  } catch (err) {
      console.error('Failed to parse response JSON', err);
      throw new Error('Invalid JSON response');
  }
};
