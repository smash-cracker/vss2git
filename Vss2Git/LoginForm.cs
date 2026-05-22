/* Copyright 2026 Dimitar Grigorov
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Hpdi.Vss2Git
{
    public class LoginForm : Form
    {
        private const string RequiredUsername = "admin";
        private const string RequiredPasswordSha256 =
            "0f50f5584f0f082459dad519d24f2de41fb9fc32c9e67f592728ac96da4770b9";

        private readonly TextBox usernameTextBox;
        private readonly TextBox passwordTextBox;
        private readonly Label messageLabel;

        public LoginForm()
        {
            Text = "Vss2Git Login";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(360, 190);
            Padding = new Padding(16);

            var usernameLabel = new Label
            {
                AutoSize = true,
                Location = new Point(16, 22),
                Text = "Username"
            };

            usernameTextBox = new TextBox
            {
                Location = new Point(110, 18),
                Size = new Size(220, 23),
                TabIndex = 0
            };

            var passwordLabel = new Label
            {
                AutoSize = true,
                Location = new Point(16, 60),
                Text = "Password"
            };

            passwordTextBox = new TextBox
            {
                Location = new Point(110, 56),
                PasswordChar = '*',
                Size = new Size(220, 23),
                TabIndex = 1,
                UseSystemPasswordChar = true
            };

            messageLabel = new Label
            {
                AutoSize = false,
                ForeColor = Color.DarkRed,
                Location = new Point(16, 94),
                Size = new Size(314, 34),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var loginButton = new Button
            {
                DialogResult = DialogResult.None,
                Location = new Point(174, 140),
                Size = new Size(75, 27),
                TabIndex = 2,
                Text = "Login"
            };
            loginButton.Click += LoginButton_Click;

            var cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(255, 140),
                Size = new Size(75, 27),
                TabIndex = 3,
                Text = "Cancel"
            };

            AcceptButton = loginButton;
            CancelButton = cancelButton;

            Controls.Add(usernameLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(messageLabel);
            Controls.Add(loginButton);
            Controls.Add(cancelButton);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (IsValidLogin(usernameTextBox.Text, passwordTextBox.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            messageLabel.Text = "Invalid username or password.";
            passwordTextBox.SelectAll();
            passwordTextBox.Focus();
        }

        private static bool IsValidLogin(string username, string password)
        {
            return string.Equals(username, RequiredUsername, StringComparison.Ordinal) &&
                string.Equals(ComputeSha256(password), RequiredPasswordSha256,
                    StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeSha256(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value ?? string.Empty));
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
