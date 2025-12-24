# SonarCloud Setup Guide

This guide explains how to set up SonarCloud integration for the VueNetCrud project.

## Prerequisites

1. A SonarCloud account (sign up at https://sonarcloud.io)
2. GitHub repository access
3. Admin access to GitHub repository settings

## Step 1: Create SonarCloud Project

1. Log in to [SonarCloud](https://sonarcloud.io)
2. Click **"+"** → **"Analyze new project"**
3. Select **"GitHub"** as your organization
4. Choose your repository: `VueNetCrud`
5. SonarCloud will generate:
   - **Organization Key** (e.g., `your-org-name`)
   - **Project Key** (e.g., `your-org-name_VueNetCrud`)

## Step 2: Generate SonarCloud Token

1. In SonarCloud, go to **"My Account"** → **"Security"**
2. Under **"Generate Tokens"**, enter a name (e.g., `GitHub Actions`)
3. Click **"Generate"**
4. **Copy the token immediately** (you won't be able to see it again)

## Step 3: Configure GitHub Secrets

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **"New repository secret"**
4. Add the following secrets:

### Required Secrets:

| Secret Name | Value | Description |
|------------|-------|-------------|
| `SONAR_TOKEN` | Your SonarCloud token | Generated in Step 2 |
| `SONAR_ORGANIZATION` | Your organization key | From SonarCloud project (e.g., `your-org-name`) |
| `SONAR_PROJECT_KEY` | Your project key | From SonarCloud project (e.g., `your-org-name_VueNetCrud`) |

## Step 4: Verify Workflow

1. Push changes to `main` or `master` branch
2. Go to **Actions** tab in GitHub
3. You should see **"SonarCloud Analysis"** workflow running
4. Once complete, check SonarCloud dashboard for analysis results

## Workflow Triggers

The SonarCloud workflow runs automatically on:
- ✅ Push to `main` or `master` branch
- ✅ Pull requests targeting `main` or `master`
- ✅ Manual trigger via **Actions** → **SonarCloud Analysis** → **Run workflow**

## Code Coverage

The workflow collects code coverage using:
- **Coverage Tool**: XPlat Code Coverage
- **Coverage Format**: OpenCover
- **Exclusions**: 
  - Test projects (`**/*.Tests.cs`)
  - Migrations (`**/Migrations/**`)
  - Entry points (`Program.cs`, `Startup.cs`)

## Troubleshooting

### Workflow Fails with "SONAR_TOKEN not found"
- Ensure you've added `SONAR_TOKEN` secret in GitHub repository settings
- Verify the token is valid in SonarCloud

### Workflow Fails with "Project key not found"
- Verify `SONAR_PROJECT_KEY` matches the project key in SonarCloud
- Check that `SONAR_ORGANIZATION` is correct

### No Code Coverage Reported
- Ensure tests are running successfully
- Check that coverage files are generated in `TestResults` directory
- Verify coverage exclusions in workflow file

### Analysis Not Appearing in SonarCloud
- Check workflow logs for errors
- Verify SonarCloud project exists and is accessible
- Ensure GitHub integration is properly configured in SonarCloud

## Additional Configuration

### Custom Analysis Properties

You can customize the analysis by modifying `.github/workflows/sonarcloud.yml`:

```yaml
args: >
  -Dsonar.projectKey=${{ secrets.SONAR_PROJECT_KEY }}
  -Dsonar.organization=${{ secrets.SONAR_ORGANIZATION }}
  -Dsonar.cs.opencover.reportsPaths=**/coverage.opencover.xml
  -Dsonar.coverage.exclusions=**/Migrations/**,**/Program.cs
  -Dsonar.exclusions=**/bin/**,**/obj/**
```

### Quality Gates

Configure quality gates in SonarCloud:
1. Go to **Quality Gates** in SonarCloud
2. Create or modify a quality gate
3. Set thresholds for:
   - Code Coverage
   - Duplicated Lines
   - Maintainability Rating
   - Security Rating
   - Reliability Rating

## Resources

- [SonarCloud Documentation](https://docs.sonarcloud.io/)
- [SonarCloud GitHub Action](https://github.com/SonarSource/sonarcloud-github-action)
- [.NET Analysis on SonarCloud](https://docs.sonarcloud.io/languages/dotnet/)

