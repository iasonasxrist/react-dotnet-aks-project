# 🚀 ASP.NET Core + PostgreSQL + ArgoCD Deployment with GitOps on AKS

## This project demonstrates how to deploy a range-partitioned ASP.NET Core API using PostgreSQL and ArgoCD for GitOps delivery in an AKS environment.

## 📂 Repository

GitHub: [sharded-based-api-database](https://github.com/iasonasxrist/sharded-based-api-database)

---

## 🧰 Technologies Used

| Tool           | Purpose                    |
| -------------- | -------------------------- |
| ASP.NET Core   | REST API backend           |
| EF Core        | ORM with PostgreSQL        |
| PostgreSQL     | Range partitioned DB       |
| ArgoCD         | GitOps-based Kubernetes CD |
| AKS            | Azure Kubernetes Service   |
| GitHub Actions | CI/CD automation           |
| ACR            | Azure Container Registry   |

---

## Overview

This project demonstrates a shared database architecture with range-based partitioning using ASP.NET Core, PostgreSQL, and a Movies API. In this scenario, the partitioning is based on the `Id` of the `Movie` entity. This might be used to ensure data is distributed evenly across partitions, particularly if the `Id` values are generated in a sequential manner.

## Architecture

### Shared Database

- Multiple "logical tenants" (data segments based on ID ranges) share the same database instance.
- Data segregation is managed via partitioning based on an `Id` range.

### Range-Based Partitioning (PostgreSQL)

- Tables are partitioned based on ranges of `Movie.Id` values (UUID in this case).
- Each partition contains movies whose `Id` falls within a specific range.
- This partitioning aims to optimize query performance and manage large datasets.

### ASP.NET Core API

- Provides RESTful endpoints for managing movies data.
- The API must be aware of the ID ranges for different partitions when inserting or updating data.
- Dynamic connection string generation is _not_ typically required, but the database context needs to properly target the correct partition for operations.
- Data access layer (e.g., Entity Framework Core) interacts with the partitioned tables.

### Microsoft SQL Server Database

- Database server hosting the partitioned movie data.
- Partitions managed using PostgreSQL's built-in partitioning features.

## Prerequisites

1.  **.NET SDK:** Download and install the .NET SDK ([https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)).
2.  **PostgreSQL:** Install PostgreSQL database server ([https://www.postgresql.org/download/](https://www.postgresql.org/download/)).
3.  **IDE:** Visual Studio or Visual Studio Code with C# extension.

## Setup and Configuration

### 1. Clone the Repository

```bash
git clone [repository-url]
cd [project-directory]
```

### Example of costs for development AKS

![costanalysis_charts](https://github.com/user-attachments/assets/71eebef1-736b-4227-a163-fb0c3634439f)

## Guide to give permissions to ACRregistry for Images

```az ad sp create-for-rbac \
  --name github-acr-pusher \
  --role acrpush \
  --scopes /subscriptions/subId/resourceGroups/iasonasaz204/providers/Microsoft.ContainerRegistry/registries/acr-name \
  --sdk-auth
```

### Returns Example

```
{
"clientId": "",
"clientSecret": "",
"subscriptionId": "",
"tenantId": "",
"activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
"resourceManagerEndpointUrl": "https://management.azure.com/",
"activeDirectoryGraphResourceId": "https://graph.windows.net/",
"sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
"galleryEndpointUrl": "https://gallery.azure.com/",
"managementEndpointUrl": "https://management.core.windows.net/"
}
```

## 🔐 Next Step: Add This to GitHub

## Configure GitHub Actions with Federated Credentials for Azure

### 1. Add AZURE_CREDENTIALS Secret to GitHub

After running the Azure CLI command to create a service principal:

bash
az ad sp create-for-rbac \
 --name github-acr-pusher \
 --role acrpush \
 --scopes /subscriptions/<subId>/resourceGroups/<resourceGroup>/providers/Microsoft.ContainerRegistry/registries/<acr-name> \
 --sdk-auth

🔐 Copy the full JSON output and add it to your GitHub repository:

1. Go to your repository on GitHub
2. Click Settings
3. Navigate to Secrets and variables → Actions
4. Click New secret
5. Name the secret exactly:

text
AZURE_CREDENTIALS

### 2. Configure Federated Credentials in Azure AD

1. Go to the [Azure Portal](https://portal.azure.com/)
2. Navigate to Azure Active Directory → App Registrations
3. Find and select your app (github-acr-pusher)
4. Under Certificates & secrets, go to Federated credentials
5. Click Add credential

Fill in the details:

- **Federated credential scenario:** GitHub Actions deploying Azure resources
- **Organization:** github-name
- **Repository:** repo-name
- **Entity type:** Branch
- **Branch name:** master
- **Name:** github-actions-federation

### 3. Verify GitHub Actions Workflow

Ensure your workflow file (.github/workflows/deploy.yml) includes the following:

yaml
ame: Deploy to Azure

permissions:
id-token: write # Required for OIDC
contents: read

jobs:
deploy:
runs-on: ubuntu-latest
steps: - uses: actions/checkout@v2

      - name: Azure Login
        uses: azure/login@v1
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

      # Your deployment steps here

> ✅ This setup allows GitHub Actions to authenticate with Azure using OpenID Connect (OIDC), eliminating the need to store long-lived secrets. i9 want it in markdown format

- uses: azure/login@v1
  with:
  client-id: ${{ secrets.AZURE_CLIENT_ID }}
  tenant-id: ${{ secrets.AZURE_TENANT_ID }}
  subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

## Run local Debugging ArgoCD

kubectl port-forward svc/argocd-server -n argocd 8080:443

## 🔧 Setup Instructions

### 1. 📥 Clone the Repository

```bash
git clone https://github.com/iasonasxrist/sharded-based-api-database.git
cd sharded-based-api-database
```

---

### 2. 📥 Install ArgoCD CLI (macOS)

```bash
brew install argocd
```

---

### 3. 🚢 Deploy ArgoCD to Kubernetes

```bash
kubectl apply -f argocd/
```

> Ensure `argocd/` contains ArgoCD manifests.

---

### 4. 🌍 Expose ArgoCD Server

Convert service to LoadBalancer:

```bash
kubectl patch svc argocd-server -n argocd -p '{"spec": {"type": "LoadBalancer"}}'
```

Check status:

```bash
kubectl get svc -n argocd
```

You may repeat:

```bash
kubectl get svc -n argocd
```

---

### 5. 🔑 Get ArgoCD Admin Password

```bash
kubectl -n argocd get secret argocd-initial-admin-secret \
  -o jsonpath="{.data.password}" | base64 -d && echo
```

---

### 6. 🌐 Port Forward ArgoCD UI (Optional)

```bash
kubectl port-forward svc/argocd-server -n argocd 8080:443
```

Then open: [https://localhost:8080](https://localhost:8080)

---

## 🔑 Configure Git Access for ArgoCD

### 7. 🔐 Generate SSH Deploy Key

```bash
ssh-keygen -t ed25519 -C "argocd-deploy-key" -f ./argocd_deploy_key
```

- Upload `argocd_deploy_key.pub` to GitHub → Repo Settings → Deploy Keys (Read-only).

---

### 8. ➕ Add Git Repo to ArgoCD

```bash
argocd repo add git@github.com:iasonasxrist/sharded-based-api-database.git \
  --ssh-private-key-path ./argocd_deploy_key
```

If necessary, run twice to ensure connection.

---

### 9. ↺ Force App Refresh

```bash
argocd app get my-app --refresh
```

> Replace `my-app` with the actual ArgoCD app name.

---

## 🚀 Deploy & Access Your Application

### 10. 🎯 Patch Frontend Service to LoadBalancer

```bash
kubectl patch svc frontend -n default -p '{"spec": {"type": "LoadBalancer"}}'
```

---

### 11. 🌐 Port Forward Frontend (Optional)

```bash
kubectl port-forward svc/frontend -n argocd 8080:443
```

---

## 🧪 Useful Commands

```bash
# View all services
kubectl get svc -n argocd
kubectl get svc

# Get all pods
kubectl get pods
kubectl get pods -A

# View ArgoCD password again
kubectl -n argocd get secret argocd-initial-admin-secret \
  -o jsonpath="{.data.password}" | base64 -d
```

---

## 📘 Debugging Mistyped Command (Note)

You ran:

```bash
kubectl mget pods
```

Fix:

```bash
kubectl get pods
```

---

## 📘 Next Steps

- Add Prometheus/Grafana for monitoring
- Add metrics-based autoscaling
- Secure ArgoCD with Ingress + cert-manager
- Link GitHub Actions for CI → ACR → AKS

---

## 🙌 Author

Made with 💡 by [@iasonasxrist](https://github.com/iasonasxrist)
