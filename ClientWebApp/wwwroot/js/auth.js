async function sendAuthRequest() {
    const login = document.querySelector('[data-login]').value;
    const password = document.querySelector('[data-password]').value;
    try {
        const response = await fetch("/api/auth", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ Login: login, Password: password })
        });

        if (!response.ok) throw new Error('Network error');

        const result = await response.json();
        document.getElementById("message").innerText = result.text;
    } catch (error) {
        document.getElementById("message").innerText = "Authorization failed";
        console.error("Auth error:", error);
    }
}

async function sendRegRequest() {
    const login = document.querySelector('[data-login]').value;
    const name = document.querySelector('[data-name]').value;
    const password = document.querySelector('[data-password]').value;
    const passwordRepeat = document.querySelector('[data-password-repeat]').value;

    if (password != passwordRepeat) {
        const result = "Passwords don't match";
        document.getElementById("message").innerText = result.text;
    }


    try {
        const response = await fetch("/api/reg", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ Login: login, Name: name, Password: password, PasswordRepeat: passwordRepeat })
        });

        if (!response.ok) throw new Error('Network error');

        const result = await response.json();
        document.getElementById("message").innerText = result.text;
    } catch (error) {
        document.getElementById("message").innerText = "Registaration failed";
        console.error("Auth error:", error);
    }
}