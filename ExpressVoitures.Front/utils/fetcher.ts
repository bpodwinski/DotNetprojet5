export async function fetchVehicles(url: string, headers: HeadersInit): Promise<any> {
  console.log('Fetching vehicles from backend...');
  try {
    const res = await fetch('http://192.168.1.101:5000/' + url, { headers });
    if (!res.ok) {
      console.error(`Failed to fetch: ${res.statusText}`);
      throw new Error(`Failed to fetch: ${res.statusText}`);
    }
    const data = await res.json();
    console.log('Fetched vehicles:', data);
    return data;
  } catch (error: any) {
    console.error('Error fetching vehicles:', error.message);
    throw error;
  }
}
