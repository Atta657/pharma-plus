async function loadDashboard() {
    try {
        // Expiring Medicines (KPI 1)
        const expiryRes = await fetch(`${API_BASE}/Medicine/expiring/30`);
        const expiryData = await expiryRes.json();
        document.getElementById("expiryCount").innerText = expiryData.length;

        // Dummy KPIs (until full APIs ready)
        document.getElementById("companyCount").innerText = 5;
        document.getElementById("medicineCount").innerText = expiryData.length + 20;

    } catch (error) {
        console.error("Dashboard Error:", error);
    }
}

window.onload = loadDashboard;