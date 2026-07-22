async function addMedicine() {
    const name = document.getElementById("name").value;
    const quantity = parseInt(document.getElementById("quantity").value);
    const expiryDate = document.getElementById("expiryDate").value;
    const companyId = parseInt(document.getElementById("companyId").value);

    if (!name || !quantity || !expiryDate || !companyId) {
        alert("Please fill all fields");
        return;
    }

    const response = await fetch(`${API_BASE}/Medicine/add`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            name: name,
            quantity: quantity,
            expiryDate: expiryDate,
            companyId: companyId
        })
    });

    if (response.ok) {
        alert("Medicine Added Successfully");
        loadExpiringMedicines();
    } else {
        alert("Error adding medicine");
    }
}

async function loadExpiringMedicines() {
    // AI-like expiry alerts (within 30 days)
    const response = await fetch(`${API_BASE}/Medicine/expiring/30`);
    const data = await response.json();

    const table = document.getElementById("medicineTable");
    table.innerHTML = "";

    data.forEach(med => {
        const row = `
            <tr>
                <td>${med.name}</td>
                <td>${med.quantity}</td>
                <td style="color:red;font-weight:bold;">
                    ${med.expiryDate.split("T")[0]}
                </td>
            </tr>
        `;
        table.innerHTML += row;
    });
}

// Auto load expiry medicines when page opens
window.onload = loadExpiringMedicines;