export const tijdzoneConverter = (datum: Date): string => {
    const utcDatum = new Date(Date.UTC(datum.getFullYear(), datum.getMonth(), datum.getDate()));
    return utcDatum.toISOString();
  };
  