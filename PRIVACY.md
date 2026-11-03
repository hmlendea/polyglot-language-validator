# Privacy and Personal Data

This document describes how Polyglot Language Validator handles personal data. The application is a command-line tool that validates constructed language outputs locally on the user's machine. It performs no network communication, telemetry, or external data transfers.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how Polyglot Language Validator at https://github.com/hmlendea/polyglot-language-validator handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

Polyglot Language Validator is a self-hosted command-line application. Users download, build, and run the application on their own machines. The project maintainers do not operate any hosted instance of this software.

Instance operators (users running the tool) control all aspects of their local execution, including:
- Configuration via command-line arguments
- Local file storage for input dictionaries, words, and sentences
- Console output and log handling
- Retention and deletion of any generated output files
- Access controls on their local machine

No data is sent from a self-hosted instance to project maintainers or external services. The application performs no telemetry, update checks, crash reporting, authentication, or network communication of any kind.

## 📥 Data We Handle

### Data Provided to the Application

- Constructed language dictionary data (parts of speech, lexicon entries, declension rules) provided via local XML file
- Word mappings (local word, constructed word, dimensions) provided via local JSON file
- Sentence test cases (word ID sequences and expected constructed sentences) provided via local JSON file

All input data is provided explicitly by the user through local file paths. The application does not request, collect, or process any personal data beyond what is contained in these user-supplied files.

### Data Generated or Collected by the Application

- Validation results printed to standard output (console) showing mismatches between generated and expected sentences
- Exit codes indicating validation success (0) or failure (non-zero)
- No personal data is generated, collected, logged, or persisted automatically by the application

### Data Received from Integrations

No personal data is received from integrations or third parties. The application has no built-in integrations with external services.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- Parsing dictionary XML and building in-memory language models — Dictionary data, lexicon entries, declension rules
- Parsing words JSON and sentences JSON — Word mappings, sentence test cases
- Applying declension and generation rules to construct sentences — Dictionary rules, word dimensions
- Comparing generated output with expected output — Generated sentences, expected sentences
- Reporting validation results to console — Mismatch details, pass/fail status

## 🗄️ Storage, Retention, and Deletion

All data processing occurs in memory during application execution. The application:
- Reads input files from user-specified local paths
- Builds temporary in-memory representations for validation
- Writes validation results to standard output (console)
- Does not create, modify, or persist any files beyond the user's input files
- Does not maintain databases, caches, logs, or backups

Instance operators control the retention and deletion of their input files and any output they choose to capture from the console. The project maintainers have no access to, control over, or visibility into any user data.

## 🔗 External Processing and Integrations

The application has no built-in external data transfers, integrations, or third-party processors. All processing occurs locally on the user's machine.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/polyglot-language-validator/blob/main/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/polyglot-language-validator/issues. For a self-hosted instance, contact the instance operator (the user running the tool). Include the repository URL and a description of the data flow in question; do not send passwords, access tokens, or other secrets.