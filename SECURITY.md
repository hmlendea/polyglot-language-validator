# Security Policy

This document describes the security policy for Polyglot Language Validator, a command-line tool for validating constructed language outputs. The application runs locally with no network communication, external dependencies, or attack surface beyond local file processing.

## 📑 Table of Contents

- [Supported Versions](#supported-versions)
- [Reporting a Vulnerability](#reporting-a-vulnerability)
- [Scope](#scope)
- [Disclosure Policy](#disclosure-policy)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | GitHub Releases | ✅ |
| Latest version | Source build | ✅ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/polyglot-language-validator/security/advisories)
- Contact the maintainers directly

## 📌 Scope

The original maintainers are not responsible for changes, modifications, or security issues introduced by downstream forks.

The subsequent report categories are in scope for this repository:
- Local file parsing vulnerabilities (XML, JSON input processing)
- Command-line argument handling issues
- Memory safety issues in .NET runtime dependencies

The subsequent categories are out of scope unless explicitly stated to the contrary:
- Vulnerabilities in user-provided input files (dictionary, words, sentences)
- Issues in third-party NuGet packages (report to package maintainers)
- .NET runtime vulnerabilities (report to Microsoft)
- Social engineering or physical access attacks

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.