# Fix SonarCloud Token Error

## Error Message
```
Please check the property sonar.token or the environment variable SONAR_TOKEN.
```

## Solution

The workflow has been updated to pass the token both as:
1. **Environment variable** (`SONAR_TOKEN`) - for the GitHub Action
2. **Property** (`-Dsonar.token`) - explicitly in the args

## Verify Your Setup

### Step 1: Check Secret Exists
1. Go to: https://github.com/ananthcooldev/vue-server/settings/secrets/actions
2. Verify `SONAR_TOKEN` secret exists
3. If missing, add it:
   - **Name**: `SONAR_TOKEN`
   - **Value**: Your SonarCloud token (from https://sonarcloud.io → My Account → Security → Generate Tokens)

### Step 2: Verify Token Format
- Token should start with `sqp_` (SonarCloud token format)
- Token should have analysis permissions
- Token should not be expired

### Step 3: Re-run Workflow
After verifying the secret exists, re-run the workflow:
1. Go to **Actions** tab
2. Find the failed **"SonarCloud Analysis"** workflow
3. Click **"Re-run all jobs"**

## What Was Changed

The workflow now explicitly passes the token in two ways:

```yaml
env:
  SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}  # Environment variable

args: >
  -Dsonar.token=${{ secrets.SONAR_TOKEN }}  # Explicit property (NEW)
  -Dsonar.projectKey=${{ secrets.SONAR_PROJECT_KEY }}
  -Dsonar.organization=${{ secrets.SONAR_ORGANIZATION }}
  ...
```

This ensures the token is available both as an environment variable (for the GitHub Action) and as a property (for the SonarCloud scanner).

## Troubleshooting

### "SONAR_TOKEN not found"
- Secret doesn't exist in GitHub
- Secret name is misspelled (must be exactly `SONAR_TOKEN`)
- Secret is in wrong section (should be Repository secrets, not Environment secrets)

### "Invalid token"
- Token is expired - generate a new one
- Token doesn't have correct permissions
- Token was copied incorrectly (missing characters)

### Still Getting Error?
1. Double-check secret exists: https://github.com/ananthcooldev/vue-server/settings/secrets/actions
2. Verify token is valid in SonarCloud
3. Check workflow logs for more detailed error messages

