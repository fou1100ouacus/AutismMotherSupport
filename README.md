# 🌸 Autism Support

**AI-Powered Daily Companion Web App for Mothers of Children with Autism**  
 
### ⚠️ Core Principle – Shown on Every Important Screen

> **This is NOT medical software.**  
> It is **NOT** a substitute for professional therapy, diagnosis, or medical care.  
> A gentle daily support tool created with love for mothers — including safe AI help only.  
> The AI chatbot gives encouragement, ideas & reflections — **never medical advice**.  
> Always consult qualified professionals for any health, behavior or development concerns.

## 🌟 1. Product Overview

A secure web app made **exclusively for mothers** to support their autistic children every day:

- 🤝 Mandatory mother account + strong legal disclaimer acceptance  
- 👶 One child profile only (strict rule)  
- 📅 Simple daily tracking (sleep, eating, meltdowns, positive moments…)  
- 🎥 Private short calm video uploads  
- 📚 Practical non-medical resource library  
- 💬 Heavily moderated emotional support community  
- 🤖 **Safe AI Chatbot** – daily encouragement, calm-down ideas, gentle parenting tips  
- 📈 **AI Gentle Progress Insights** – positive patterns from history (safe text only, no diagnosis)

**Daily usage goal** → 2–5 minutes quick check-in
 
## 🛤️ 2. Complete User Flow – Deep Version (From Mother’s View)

### 🚪 Phase 0 – Anonymous Visitor (Before First Use)

1. Opens website (usually mobile browser in Egypt)  
   → Very strong repeated disclaimer  
   → Clear Arabic/English toggle (full RTL support)  
   → Big button: **“Start Supporting My Child”** → Register

   **Backend:** Serves only static/public content (no login needed)

### 🔐 Phase 1 – Account Creation + Mandatory Disclaimer (Critical Safety Gate)

2. Clicks **Register**  
   → Form: Email + Password

3. Forced disclaimer screen (cannot be skipped)  
   → Long clear text explaining the core principle  
   → Checkbox: “I fully understand and accept that this is NOT medical software…”  
   → **“Confirm”** button (acts like digital signature)

   **Backend:**  
   • Creates User record (role = Mother, IsActive = false)  
   • On confirm → sets DisclaimerAcceptedAt = now, IsActive = true  
   • Returns JWT token  
   • Logs to AuditLogs: action=DisclaimerAccepted

4. If she tries to skip or close → session ends, must start over

### 👶 Phase 2 – First-Time Setup (Child Profile – Gate to All Features)

5. After disclaimer → redirected to **“Create Your Child’s Profile”**  
   • **Required:** Nickname, AgeYears, AgeMonths  
   • **Optional:** Gender, SupportNeedsLevel (Light/Medium/High), Challenges (tags), Strengths (text), preferences…

   **Backend:**  
   • POST /api/children  
   • Validates required fields  
   • One-child rule → 409 Conflict if profile already exists  
   • Inserts record with UserId foreign key  
   • Returns 201 Created + ChildId  
   • Logs to AuditLogs

6. Trying to access dashboard / tracking / community without profile  
   → 403 Forbidden + message: “Please complete your child’s profile first”

### 🌞 Phase 3 – Daily Core Loop (What most mothers do every day)

7. Logs in (email/password or refresh token)  
   → JWT validated → session active → lands on personalized dashboard

8. Dashboard shows:  
   • Today’s date + “How was today for [Nickname]?”  
   • Quick mother mood picker (stress 1–10 + optional emoji)  
   • Button: “Add Today’s Notes”

9. **Daily Tracking Entry** (one per day)  
   • Sections: Sleep, Eating, Meltdowns, Communication, Positive Moments, Sensory, Strategies…  
   • Tags pre-filled from child profile  
   • Submit

   **Backend:**  
   • POST /api/tracking  
   • Verifies ChildId belongs to user  
   • Enforces one entry per day (ChildId + Date unique)  
   • Saves data (JSON)  
   • Returns success + warm message (“Thank you for tracking today ❤️”)

10. **View History**  
    • Calendar or list of past days  
    • Click day → full details

    **Backend:** GET /api/tracking?childId=xxx&from=…&to=…

11. **Video Moment Upload** (optional but encouraged)  
    • “Upload a short video of a calm moment today”  
    • Drag-drop or select (<5 min enforced by frontend)  
    • Optional private note (e.g. “He loved the weighted blanket”)

    **Backend:**  
    • POST /api/videos/upload → returns presigned URL (Azure Blob / S3)  
    • Frontend uploads directly  
    • Callback → inserts VideoUploads record (Status=Uploaded)  
    • Publishes “VideoUploaded” event  
    • Mother sees: “Video received – we’ll process it soon”

### 📚 Phase 4 – Resource Discovery & Inspiration

12. Goes to **“Resources”** tab  
    • Categories: Visual Schedules, Calm Corner Ideas, Play Activities, Mother Self-Care, Books/Channels  
    • Search or browse cards → description + link/print/download

    **Backend:** GET /api/resources?category=…

### 💬 Phase 5 – Community Interaction (Social Safety Layer)

13. Goes to **“Community”** tab  
    • Feed of **Approved** posts only (latest first, paginated)  
    • Each post shows: text, photo (if any), reactions, comments, time ago

14. Wants to post  
    • “Share something” button  
    • Text + optional photo  
    • Reminder: “Keep it practical & emotional – no medical terms”  
    • Submit → “Your post is under review – thank you for sharing safely ❤️”

    **Backend:**  
    • POST /api/posts  
    • Keyword blocklist scan (regex)  
    • Clean → Status = Pending  
    • Blocked → reject + message  
    • Stores photo if provided  
    • Logs action

15. Interacts with posts  
    • React (Heart / Hug / Pray / ThumbsUp) → immediate  
    • Comment → goes to Pending  
    • Report button (if something feels wrong)

    **Backend:**  
    • POST /api/reactions (immediate)  
    • POST /api/comments (Pending)  
    • POST /api/reports → adds to moderation queue

### 🛡️ Phase 6 – Moderator / Admin Side (Not visible to mothers)

16. Moderator logs in (role = Moderator / Admin)  
    • Sees moderation queue: Pending posts & comments  
    • Reviews → Approve / Reject + optional note

    **Backend:**  
    • GET /api/admin/queue  
    • PUT /api/posts/{id}/moderate {Status, Note}  
    • Approved → becomes visible  
    • Rejected → hidden + reason logged  
    • Full audit log for every decision

### ♻️ Phase 7 – Repeated Daily Cycle (Long-term Usage)

Every day:  
→ Login → dashboard → track today → maybe upload video → browse resources → read / interact in community

Weekly / monthly:  
→ Check history → see gentle patterns (external AI → mother sees only safe positive text)

Daily:  
→ Receives small encouragement tip (banner or notification)

**Backend background:**  
• Scheduled job calls external AI for safe tip  
• Displays only filtered, positive message


## ✨ Key Features

### For Mothers
- **Mandatory Safety Gate** – Disclaimer acceptance (acts as digital signature)
- **Single Child Profile** – Nickname, age, challenges, strengths, preferences
- **Daily Tracking** – Sleep, eating, meltdowns, communication, positive moments, sensory, strategies (one entry per day)
- **Mother’s Mood Checker** – Quick stress level (1–10) + emoji
- **Video Moments** – Upload short calm videos (<5 min) with private notes
- **Resources Library** – Visual schedules, calm corner ideas, play activities, mother self-care, books/channels
- **Safe Community** – Read approved posts, react, comment, post (all pending moderation)
- **History & Gentle Patterns** – Calendar view + weekly/monthly insights (external AI → safe text only)
- **Daily Encouragement Tips** – In-app banner or notification

### For Moderators/Admins
- Moderation queue for posts, comments, and reports
- Keyword blocklist + manual Approve/Reject
- Full audit logging

---

## 🔄 Complete User Flow (Mother’s Perspective)

```mermaid
flowchart TD
    A[Landing Page<br>Strong Disclaimer + AR/EN Toggle] --> B[Register]
    B --> C[Forced Disclaimer Screen<br>Non-skippable Checkbox]
    C --> D[Create Child Profile<br>One child only]
    D --> E[Dashboard Daily Hub]
    
    E --> F[Daily Tracking Entry]
    E --> G[Upload Video Moment]
    E --> H[View History & Patterns]
    E --> I[Resources Library]
    E --> J[Community Feed]
    
    J --> K[Create Post → Pending Review]
    J --> L[React / Comment → Pending]
    
    subgraph Moderator
    M[Moderation Queue] --> N[Approve / Reject]
    end
Detailed 12-phase breakdown is in the attached PDF (English pages 1–6, Arabic pages 7–12).

🛠️ Technical Architecture Highlights
Backend (Non-AI Core)

REST API endpoints (/api/children, /api/tracking, /api/videos/upload, /api/posts, /api/resources, etc.)
JWT Authentication + role-based access (Mother / Moderator / Admin)
Strict validation:
One child per user (409 Conflict)
One tracking entry per day
All community content starts as Pending

AuditLogs for every action (DisclaimerAccepted, tracking, uploads, moderation decisions)
Blob storage (Azure Blob / S3) with presigned URLs for videos & photos
Scheduled jobs for daily tips (external AI → safe text only)

Frontend

Mobile browser optimized (Egypt traffic)
Full Arabic/English toggle with RTL support
Drag & drop video upload (<5 min enforced)
Personalized dashboard as daily hub
Tag system pre-populated from child profile

Security & Safety Layers

Repeated strong disclaimers
Non-skippable acceptance
One-child rule
Keyword blocklist + human moderation
All posts/comments start hidden
Report system


📁 Project Structure (Recommended)
textproject-autism/
├── docs/
│   └── 2- Project flow in deep english and arabic.pdf
├── backend/          # .NET / Node.js / etc.
├── frontend/         # React / Next.js / etc. (mobile-first, RTL)
├── moderation-queue/
└── README.md

🚀 Getting Started (for Developers)

Review the full flow PDF (this is the single source of truth).
Implement in the prioritized order shown in the document.
Start with:
Landing + Registration + Disclaimer
Child Profile (one-child enforcement)
Daily Tracking

Use the exact endpoint names and response behaviors described.

Need a specific part expanded?
Video upload lifecycle? Moderation flow as Mermaid diagram? Database schema? Just tell me which section!

❤️ Core Principle (Repeated Everywhere)
This is NOT medical software.
It is a practical daily support tool created with love for mothers.
Project created for Aya and all the incredible mothers in Egypt ❤️

Last updated: March 2026
Version: Flow Specification v1.0
text**Copy the entire content above into a new file named `README.md`** in your project root. It is ready to use, professional, bilingual-friendly, and directly based on the provided deep flow document. Let me know if you want a shorter version, a version in Arabic only, or additional sections (API examples, database schema, etc.)!1.4sFastcreate a prd for this as a readme fileMarkdown# Project Autism - Product Requirements Document (PRD)

**Product Name:** Project Autism  
**Version:** 1.0  
**Date:** March 2026  
**Status:** Approved for Implementation  
**Primary User:** Mothers of children with autism (Egypt-focused)  
**Language Support:** Full Bilingual (Arabic/English) with RTL  
**Platform:** Mobile-first Web Application (Progressive Web App ready)  

**Core Principle (Repeated in Every Screen):**  
> This is **NOT** medical software. It is a practical daily support tool created with love for mothers.

---

## 📋 1. Executive Summary

**Project Autism** is a secure, non-AI core web platform built exclusively for mothers to support their autistic children through **daily routines**, **emotional encouragement**, and a **safe, moderated community**.

- **Focus:** Quick daily check-ins (2–5 minutes), one child per mother, zero medical claims.
- **Safety-First Design:** Mandatory disclaimer, child profile gate, content moderation, audit logs.
- **Egypt-Centric:** Mobile browser optimized, Arabic priority, culturally relevant resources.
- **Non-AI Backend:** All core features (tracking, community, resources) run on standard backend. External AI used **only** for safe text outputs (tips, patterns) visible to mothers.

**Business Goal:** Reduce maternal isolation, build daily habits, foster peer support in a moderated space.

**Key Differentiator:** Everything is gated behind safety layers — no shortcuts.

**Full User Flow Reference:** [2- Project flow in deep english and arabic.pdf](docs/2-%20Project%20flow%20in%20deep%20english%20and%20arabic.pdf)

---

## 🎯 2. Problem Statement & Opportunity

### Problems for Target Mothers
- Daily overwhelm without structured, practical tracking.
- Isolation — lack of safe, non-judgmental community.
- Flood of medical/complex advice vs. need for simple, actionable tools.
- No single "daily hub" for quick wins (mood, tracking, inspiration).
- Risk of unsafe online spaces (unmoderated groups).

### Opportunity
A **daily 2-minute ritual** app that feels like a supportive friend: track wins, share safely, get gentle tips, connect with other moms.

---

## 👥 3. Target Audience & Personas

### Primary Persona: "Umm Ahmed" (Mother, 32, Mansoura, Egypt)
- **Demographics:** 25–45 years, mobile-only, Arabic primary, low-to-mid tech comfort.
- **Needs:** Quick entry, emotional validation, practical ideas, one-child focus.
- **Pain Points:** Time scarcity, fear of "wrong" advice, desire for peer stories.
- **Goals:** Feel supported daily, see patterns in child's day, contribute safely.

### Secondary Persona: "Moderator Fatima" (Experienced Mother/Admin)
- Reviews content to keep community safe.

**Market Size:** 100k+ families in Egypt (estimated autism prevalence).

---

## 🛠️ 4. Product Goals & Success Metrics

### Business Goals
- 80% of users complete daily tracking within 7 days of onboarding.
- 60% retention at 30 days.
- 90% disclaimer acceptance rate.
- Community: 70% of posts approved within 24 hours.

### User Goals
- Mothers feel "seen and safe" (NPS > 8).
- Daily habit formation (tracking streak visible).
- Resource discovery leading to real-world use.

### KPIs
- Daily Active Users (DAU)
- Tracking entries per user/week
- Video uploads/month
- Community engagement (reactions + comments)
- Moderator queue clearance time

---

## 🔄 5. User Flows (High-Level)

### Onboarding Flow (Phases 0–2)
```mermaid
flowchart TD
    A[Landing: Strong Disclaimer + AR/EN] --> B[Register: Email + Password]
    B --> C[Forced Disclaimer: Non-skippable Checkbox]
    C --> D[Child Profile: Required fields, One-child rule]
    D --> E[Dashboard: Personalized Daily Hub]
Daily Core Loop (Phase 3 + 7)
#mermaid-diagram-mermaid-jbbiowf{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;fill:#ccc;}@keyframes edge-animation-frame{from{stroke-dashoffset:0;}}@keyframes dash{to{stroke-dashoffset:0;}}#mermaid-diagram-mermaid-jbbiowf .edge-animation-slow{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 50s linear infinite;stroke-linecap:round;}#mermaid-diagram-mermaid-jbbiowf .edge-animation-fast{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 20s linear infinite;stroke-linecap:round;}#mermaid-diagram-mermaid-jbbiowf .error-icon{fill:#a44141;}#mermaid-diagram-mermaid-jbbiowf .error-text{fill:#ddd;stroke:#ddd;}#mermaid-diagram-mermaid-jbbiowf .edge-thickness-normal{stroke-width:1px;}#mermaid-diagram-mermaid-jbbiowf .edge-thickness-thick{stroke-width:3.5px;}#mermaid-diagram-mermaid-jbbiowf .edge-pattern-solid{stroke-dasharray:0;}#mermaid-diagram-mermaid-jbbiowf .edge-thickness-invisible{stroke-width:0;fill:none;}#mermaid-diagram-mermaid-jbbiowf .edge-pattern-dashed{stroke-dasharray:3;}#mermaid-diagram-mermaid-jbbiowf .edge-pattern-dotted{stroke-dasharray:2;}#mermaid-diagram-mermaid-jbbiowf .marker{fill:lightgrey;stroke:lightgrey;}#mermaid-diagram-mermaid-jbbiowf .marker.cross{stroke:lightgrey;}#mermaid-diagram-mermaid-jbbiowf svg{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;}#mermaid-diagram-mermaid-jbbiowf p{margin:0;}#mermaid-diagram-mermaid-jbbiowf .label{font-family:"trebuchet ms",verdana,arial,sans-serif;color:#ccc;}#mermaid-diagram-mermaid-jbbiowf .cluster-label text{fill:#F9FFFE;}#mermaid-diagram-mermaid-jbbiowf .cluster-label span{color:#F9FFFE;}#mermaid-diagram-mermaid-jbbiowf .cluster-label span p{background-color:transparent;}#mermaid-diagram-mermaid-jbbiowf .label text,#mermaid-diagram-mermaid-jbbiowf span{fill:#ccc;color:#ccc;}#mermaid-diagram-mermaid-jbbiowf .node rect,#mermaid-diagram-mermaid-jbbiowf .node circle,#mermaid-diagram-mermaid-jbbiowf .node ellipse,#mermaid-diagram-mermaid-jbbiowf .node polygon,#mermaid-diagram-mermaid-jbbiowf .node path{fill:#1f2020;stroke:#ccc;stroke-width:1px;}#mermaid-diagram-mermaid-jbbiowf .rough-node .label text,#mermaid-diagram-mermaid-jbbiowf .node .label text,#mermaid-diagram-mermaid-jbbiowf .image-shape .label,#mermaid-diagram-mermaid-jbbiowf .icon-shape .label{text-anchor:middle;}#mermaid-diagram-mermaid-jbbiowf .node .katex path{fill:#000;stroke:#000;stroke-width:1px;}#mermaid-diagram-mermaid-jbbiowf .rough-node .label,#mermaid-diagram-mermaid-jbbiowf .node .label,#mermaid-diagram-mermaid-jbbiowf .image-shape .label,#mermaid-diagram-mermaid-jbbiowf .icon-shape .label{text-align:center;}#mermaid-diagram-mermaid-jbbiowf .node.clickable{cursor:pointer;}#mermaid-diagram-mermaid-jbbiowf .root .anchor path{fill:lightgrey!important;stroke-width:0;stroke:lightgrey;}#mermaid-diagram-mermaid-jbbiowf .arrowheadPath{fill:lightgrey;}#mermaid-diagram-mermaid-jbbiowf .edgePath .path{stroke:lightgrey;stroke-width:2.0px;}#mermaid-diagram-mermaid-jbbiowf .flowchart-link{stroke:lightgrey;fill:none;}#mermaid-diagram-mermaid-jbbiowf .edgeLabel{background-color:hsl(0, 0%, 34.4117647059%);text-align:center;}#mermaid-diagram-mermaid-jbbiowf .edgeLabel p{background-color:hsl(0, 0%, 34.4117647059%);}#mermaid-diagram-mermaid-jbbiowf .edgeLabel rect{opacity:0.5;background-color:hsl(0, 0%, 34.4117647059%);fill:hsl(0, 0%, 34.4117647059%);}#mermaid-diagram-mermaid-jbbiowf .labelBkg{background-color:rgba(87.75, 87.75, 87.75, 0.5);}#mermaid-diagram-mermaid-jbbiowf .cluster rect{fill:hsl(180, 1.5873015873%, 28.3529411765%);stroke:rgba(255, 255, 255, 0.25);stroke-width:1px;}#mermaid-diagram-mermaid-jbbiowf .cluster text{fill:#F9FFFE;}#mermaid-diagram-mermaid-jbbiowf .cluster span{color:#F9FFFE;}#mermaid-diagram-mermaid-jbbiowf div.mermaidTooltip{position:absolute;text-align:center;max-width:200px;padding:2px;font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:12px;background:hsl(20, 1.5873015873%, 12.3529411765%);border:1px solid rgba(255, 255, 255, 0.25);border-radius:2px;pointer-events:none;z-index:100;}#mermaid-diagram-mermaid-jbbiowf .flowchartTitleText{text-anchor:middle;font-size:18px;fill:#ccc;}#mermaid-diagram-mermaid-jbbiowf rect.text{fill:none;stroke-width:0;}#mermaid-diagram-mermaid-jbbiowf .icon-shape,#mermaid-diagram-mermaid-jbbiowf .image-shape{background-color:hsl(0, 0%, 34.4117647059%);text-align:center;}#mermaid-diagram-mermaid-jbbiowf .icon-shape p,#mermaid-diagram-mermaid-jbbiowf .image-shape p{background-color:hsl(0, 0%, 34.4117647059%);padding:2px;}#mermaid-diagram-mermaid-jbbiowf .icon-shape rect,#mermaid-diagram-mermaid-jbbiowf .image-shape rect{opacity:0.5;background-color:hsl(0, 0%, 34.4117647059%);fill:hsl(0, 0%, 34.4117647059%);}#mermaid-diagram-mermaid-jbbiowf :root{--mermaid-font-family:"trebuchet ms",verdana,arial,sans-serif;}DashboardDaily Tracking EntryUpload Calm Video MomentResources LibrarySafe Community FeedView Past DaysCreate Post → Pending
Detailed 12-Phase Flow: See PDF (English: p1–6, Arabic: p7–12).

✨ 6. Functional Requirements
Phase 0: Anonymous Visitor

Static landing with repeated disclaimers.
AR/EN toggle (RTL).

Phase 1: Onboarding & Disclaimer (Critical Safety Gate)

Register: Email + Password.
Non-skippable disclaimer screen.
Checkbox + "Confirm" (digital signature).
Backend: Create user (IsActive=false → true on accept), JWT, AuditLog.

Phase 2: Child Profile (Gate to Everything)

Required: Nickname, Age (Years + Months).
Optional: Gender, Needs Level, Challenges (multi-select), Strengths, Preferences.
One-child rule: 409 Conflict if exists.
Backend: POST /api/children, FK to User.

Phase 3: Daily Core Loop

Dashboard: Today's date + "How was today for [Nickname]?"
Mother Mood: Stress 1–10 + emoji.

Tracking Entry: Sections (Sleep, Eating, Meltdowns, etc.), pre-populated tags.
One per day: ChildId + Date unique.
POST /api/tracking.

History: Calendar/list, GET /api/tracking.
Video Upload: <5min, private note, presigned URL (Azure/S3), event queue.
POST /api/videos/upload.


Phase 4: Resources

Categories: Visual Schedules, Calm Corners, Play, Self-Care, Books.
Cards: Description + download/print.
GET /api/resources?category=...

Phase 5: Community (Social Safety Layer)

Feed: Approved posts only (latest, paginated).
Post: Text + photo, keyword scan, Status=Pending.
React/Comment: Immediate react, pending comment.
Report: To moderation queue.
POST /api/posts, /api/reactions, /api/comments, /api/reports.

Phase 6: Moderator/Admin

Queue: Pending posts/comments.
Actions: Approve/Reject + note.
Audit: Full logs.
GET /api/admin/queue, PUT /api/posts/{id}/moderate.

Phase 7: Long-Term Usage

Daily tips: Scheduled job → external AI → safe text.
Gentle patterns: History summary (AI text only).


📊 7. Non-Functional Requirements
Performance

Page load < 2s on 3G (Egypt).
API response < 500ms.

Security

JWT auth + role-based (Mother, Moderator, Admin).
All uploads: Presigned URLs.
Data: Child data encrypted at rest.
Rate limiting, CAPTCHA on register.

Accessibility & Usability

WCAG 2.1 AA.
Large buttons, high contrast.
VoiceOver friendly.
Offline-capable for tracking (PWA).

Scalability

10k users initially, auto-scale.
Blob storage for media.

Tech Stack Recommendations

Backend: .NET 8 / Node.js (REST API).
Frontend: React/Next.js (Tailwind, RTL).
DB: PostgreSQL (with JSON for tracking).
Storage: Azure Blob / S3.
Queue: RabbitMQ / Azure Queue.
Auth: JWT + Refresh.
Hosting: Azure / AWS.


🗃️ 8. Data Model (High-Level)
#mermaid-diagram-mermaid-9lmkilz{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;fill:#ccc;}@keyframes edge-animation-frame{from{stroke-dashoffset:0;}}@keyframes dash{to{stroke-dashoffset:0;}}#mermaid-diagram-mermaid-9lmkilz .edge-animation-slow{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 50s linear infinite;stroke-linecap:round;}#mermaid-diagram-mermaid-9lmkilz .edge-animation-fast{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 20s linear infinite;stroke-linecap:round;}#mermaid-diagram-mermaid-9lmkilz .error-icon{fill:#a44141;}#mermaid-diagram-mermaid-9lmkilz .error-text{fill:#ddd;stroke:#ddd;}#mermaid-diagram-mermaid-9lmkilz .edge-thickness-normal{stroke-width:1px;}#mermaid-diagram-mermaid-9lmkilz .edge-thickness-thick{stroke-width:3.5px;}#mermaid-diagram-mermaid-9lmkilz .edge-pattern-solid{stroke-dasharray:0;}#mermaid-diagram-mermaid-9lmkilz .edge-thickness-invisible{stroke-width:0;fill:none;}#mermaid-diagram-mermaid-9lmkilz .edge-pattern-dashed{stroke-dasharray:3;}#mermaid-diagram-mermaid-9lmkilz .edge-pattern-dotted{stroke-dasharray:2;}#mermaid-diagram-mermaid-9lmkilz .marker{fill:lightgrey;stroke:lightgrey;}#mermaid-diagram-mermaid-9lmkilz .marker.cross{stroke:lightgrey;}#mermaid-diagram-mermaid-9lmkilz svg{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;}#mermaid-diagram-mermaid-9lmkilz p{margin:0;}#mermaid-diagram-mermaid-9lmkilz .entityBox{fill:#1f2020;stroke:#ccc;}#mermaid-diagram-mermaid-9lmkilz .relationshipLabelBox{fill:hsl(20, 1.5873015873%, 12.3529411765%);opacity:0.7;background-color:hsl(20, 1.5873015873%, 12.3529411765%);}#mermaid-diagram-mermaid-9lmkilz .relationshipLabelBox rect{opacity:0.5;}#mermaid-diagram-mermaid-9lmkilz .labelBkg{background-color:rgba(32.0000000001, 31.3333333334, 31.0000000001, 0.5);}#mermaid-diagram-mermaid-9lmkilz .edgeLabel .label{fill:#ccc;font-size:14px;}#mermaid-diagram-mermaid-9lmkilz .label{font-family:"trebuchet ms",verdana,arial,sans-serif;color:#ccc;}#mermaid-diagram-mermaid-9lmkilz .edge-pattern-dashed{stroke-dasharray:8,8;}#mermaid-diagram-mermaid-9lmkilz .node rect,#mermaid-diagram-mermaid-9lmkilz .node circle,#mermaid-diagram-mermaid-9lmkilz .node ellipse,#mermaid-diagram-mermaid-9lmkilz .node polygon{fill:#1f2020;stroke:#ccc;stroke-width:1px;}#mermaid-diagram-mermaid-9lmkilz .relationshipLine{stroke:lightgrey;stroke-width:1;fill:none;}#mermaid-diagram-mermaid-9lmkilz .marker{fill:none!important;stroke:lightgrey!important;stroke-width:1;}#mermaid-diagram-mermaid-9lmkilz :root{--mermaid-font-family:"trebuchet ms",verdana,arial,sans-serif;}has (1:1)has (1:N)has (1:N)has (1:N)belongs tohashasreferencesreferencesUsersChildProfilesTrackingEntriesVideoUploadsPostsCommentsReactionsModerationQueue
Key Entities:

Users: Id, Email, Role, DisclaimerAcceptedAt, IsActive.
ChildProfiles: Id, UserId, Nickname, AgeYears, AgeMonths, Challenges (JSON).
TrackingEntries: Id, ChildId, Date, Data (JSON).
Posts/Comments: Status (Pending/Approved/Rejected), Content.


🎨 9. UI/UX Guidelines

Design System: Clean, warm colors (soft blues/greens), Arabic fonts (e.g., Cairo).
Navigation: Bottom tab bar (Dashboard, Tracking, Resources, Community).
Microcopy: Empathetic, encouraging (e.g., "Thank you for tracking today ❤️").
Error States: Gentle (e.g., "Please complete profile first").
Mockups: Figma link (to be created).


🚧 10. Assumptions & Dependencies

Assumptions: Users are mothers (self-identified via onboarding). External AI is safe/filtered.
Dependencies: Blob storage, Email service (for verification), AI API (for tips).
Out of Scope: Full mobile app (PWA sufficient), Multi-child support, Medical integrations.


⚠️ 11. Risks & Mitigations









































RiskLikelihoodImpactMitigationUsers skipping disclaimerHighHighNon-skippable + session killUnsafe community contentMediumHighKeyword block + human mod + reportsLow adoptionMediumHighDaily tips, simple UX, Egypt marketingData privacy concernsLowHighStrong disclaimers, no PII sharingAI output inappropriateLowMediumHuman review + safe prompt engineering

📅 12. Implementation Roadmap (Prioritized)
MVP (v1.0) – 6 Weeks

Landing → Register → Disclaimer → Child Profile.
Dashboard + Daily Tracking.
History View.

Phase 2 (v1.1) – 4 Weeks
4. Video Upload.
5. Resources Library.
6. Basic Community (post + feed).
Phase 3 (v1.2) – 4 Weeks
7. Moderation Queue + Admin.
8. Reactions/Comments.
9. Daily Tips + Patterns.
Future (v2.0)

Notifications, Analytics dashboard, Printable resources.

Prioritization: Based on PDF flow order.

📚 References

Deep User Flow PDF
Figma Designs (TBD)
API Spec (Swagger – to be generated)

This PRD is the single source of truth.
Any changes require update here + stakeholder approval.
Created with ❤️ for Aya and all the mothers in Egypt.

Last Updated: March 03, 2026
Next Review: April 2026
text**Copy the entire content above into a new file named `PRD.md` (or replace your existing `README.md`).**  

This is a complete, professional PRD tailored to the deep flow document. It's actionable for developers, clear for stakeholders, and fully aligned with the project's safety-first, mother-centric vision.  

If you need:
- Arabic version
- Detailed API specs
- Figma wireframes description
- Database schema (full SQL)
- Or adjustments... just say the word!1 / 21.6sFastadd technologis will used such sql server , c sharp ,asp.net , java scriptUpdated PRD – Now with Specified Technology Stack
Markdown# Project Autism - Product Requirements Document (PRD)

**Product Name:** Project Autism  
**Version:** 1.0  
**Date:** March 03, 2026  
**Status:** Approved for Implementation  
**Primary User:** Mothers of children with autism (Egypt-focused)  
**Language Support:** Full Bilingual (Arabic/English) with RTL  
**Platform:** Mobile-first Web Application (Progressive Web App ready)  

**Core Principle (Repeated in Every Screen):**  
> This is **NOT** medical software. It is a practical daily support tool created with love for mothers.

---

## 📋 1. Executive Summary

**Project Autism** is a secure, non-AI core web platform built exclusively for mothers to support their autistic children through **daily routines**, **emotional encouragement**, and a **safe, moderated community**.

- **Focus:** Quick daily check-ins (2–5 minutes), one child per mother, zero medical claims.
- **Safety-First Design:** Mandatory disclaimer, child profile gate, content moderation, audit logs.
- **Egypt-Centric:** Mobile browser optimized, Arabic priority, culturally relevant resources.
- **Non-AI Backend:** All core features run on standard backend. External AI used **only** for safe text outputs.

**Business Goal:** Reduce maternal isolation, build daily habits, foster peer support in a moderated space.

**Full User Flow Reference:** [2- Project flow in deep english and arabic.pdf](docs/2-%20Project%20flow%20in%20deep%20english%20and%20arabic.pdf)

---

## 🛠️ 7. Non-Functional Requirements (Updated with Chosen Technologies)

### Tech Stack (Final Decision)

| Layer          | Technology                          | Reason |
|----------------|-------------------------------------|--------|
| **Backend**    | **ASP.NET Core 8** (C#)            | High performance, excellent security, built-in JWT, dependency injection, and easy integration with SQL Server |
| **Database**   | **Microsoft SQL Server**           | Enterprise-grade, native JSON support for tracking data, full-text search for moderation, stored procedures for audit logs |
| **Frontend**   | **JavaScript** + **React.js** (or Next.js 15) | Fast, mobile-first, excellent RTL/Arabic support, component-based, easy state management |
| **ORM**        | Entity Framework Core 8            | Code-first, migrations, LINQ queries |
| **Authentication** | JWT + ASP.NET Core Identity     | Secure token-based auth with refresh tokens |
| **File Storage** | Azure Blob Storage                | Presigned URLs for video & photo uploads |
| **Background Jobs** | Hangfire (with SQL Server)     | Daily encouragement tips, video processing queue |
| **UI Framework** | Tailwind CSS + DaisyUI / Radix UI | Beautiful, accessible, RTL-ready components |
| **Hosting**    | Azure App Service (Windows)        | Native SQL Server + ASP.NET support |
| **Version Control** | Git + GitHub                     | Standard |
| **API Documentation** | Swagger / OpenAPI                | Auto-generated |

**Why this stack?**
- Full Microsoft ecosystem (C#, ASP.NET, SQL Server) → best performance, security, and maintainability for this project.
- JavaScript + React on frontend → modern, responsive, easy for any Egyptian developer to join.
- All technologies are production-proven and free for startup phase.

---

## 🗃️ 8. Data Model (High-Level) – SQL Server Ready

```sql
-- Example tables (Entity Framework Code-First) 
Users
├── Id (PK)
├── Email
├── PasswordHash
├── Role (Mother/Moderator/Admin)
├── DisclaimerAcceptedAt
├── IsActive

ChildProfiles
├── Id (PK)
├── UserId (FK)
├── Nickname
├── AgeYears
├── AgeMonths
├── Challenges (JSON)
├── Strengths (NVARCHAR(MAX))

TrackingEntries
├── Id (PK)
├── ChildId (FK)
├── EntryDate (unique per child)
├── Data (JSON)        -- Sleep, Eating, Meltdowns, etc.
├── CreatedAt

VideoUploads
├── Id (PK)
├── ChildId (FK)
├── BlobUrl
├── Status (Uploaded/Processed)
├── PrivateNote

Posts, Comments, Reactions, AuditLogs, ModerationQueue...

📅 12. Implementation Roadmap (With Technologies)
MVP (v1.0) – 6 Weeks

ASP.NET Core Web API + SQL Server Database
React.js Frontend (Vite or Next.js)
Landing → Register → Disclaimer → Child Profile (C# + React)
Dashboard + Daily Tracking (EF Core + React forms)

Phase 2 (v1.1) – 4 Weeks
5. Video Upload (presigned Azure Blob + Hangfire)
6. Resources Library
7. Community (posts with keyword filtering)
Phase 3 (v1.2) – 4 Weeks
8. Moderator Dashboard (ASP.NET MVC or React Admin panel)
9. Full Audit Logging + Reports
10. Daily Tips Scheduler (Hangfire + external AI safe call)

✅ All Other Sections Remain the Same
(Executive Summary, Problem Statement, User Flows, Functional Requirements, Risks, etc. are unchanged and fully compatible with the chosen C# / ASP.NET / SQL Server / JavaScript stack.)
This PRD is the single source of truth.
Any changes require update here + stakeholder approval.
Created with ❤️ for Aya and all the mothers in Egypt.

Last Updated: March 03, 2026
Tech Stack Locked: C#, ASP.NET Core 8, SQL Server, JavaScript + React.js
text---

**How to use:**

1. Copy everything above.
2. Save as `PRD.md` (or replace your existing one).
3. The technology section is now clear, professional, and exactly matches your request: **SQL Server + C# + ASP.NET + JavaScript**.

Would you like me to also create:
- A separate `TECHNICAL_DESIGN.md` with folder structure?
- Entity Framework models examples?
- Or the Arabic version of this PRD?

Just say the word! ❤️1.5sFastedit table format to be readableHere is a cleaned-up, more readable version of the project flow.
I restructured it into clear numbered phases with consistent formatting:

Phase headers
Step numbers
Frontend / User actions (plain text)
Backend actions (indented with • bullet points)

Complete User Flow – Deep Version (Mother’s Perspective)
Phase 0 – Before First Use (Anonymous Visitor)

Opens website (mobile browser – most likely in Egypt)
Sees landing page with very strong repeated disclaimer
Clear Arabic/English toggle (full RTL support)
Big buttons: «ابدئي بدعم طفلي» / “Start Supporting My Child” → Register
Backend at this stage: Serves static/public content only (no authentication required)

Phase 1 – First Visit & Onboarding (Critical Safety Gate)

Clicks Register
Form: Email + Password
Immediately after submit → forced disclaimer screen

Disclaimer Acceptance (non-skippable)
Long clear text (core principle: this is NOT medical software)
Checkbox: «أفهم تماماً وأقبل أن هذا ليس برنامجاً طبياً...»
“I fully understand and accept that this is NOT medical software…”
“Confirm” button (acts like digital signature)
Backend:
Creates User record (role = Mother, IsActive = false until disclaimer)
On confirm → sets DisclaimerAcceptedAt = now, IsActive = true
Returns JWT token
Logs to AuditLogs: action=DisclaimerAccepted

If user tries to skip or close the disclaimer screen
→ session ends, must start over from beginning

Phase 2 – First-Time Setup (Child Profile – Gate to Everything Else)

After disclaimer → redirected to “Create Your Child’s Profile” screen
Required: Nickname, AgeYears, AgeMonths
Optional: Gender, SupportNeedsLevel (Light/Medium/High), Challenges (multi-select tags), Strengths (text), VisualSchedulePref, CommunicationMethods (tags)
“Save & Continue” button
Backend:
POST /api/children
Validates required fields
Checks if User already has a ChildProfile → 409 Conflict if yes (one-child rule)
Inserts record with UserId foreign key
Returns 201 Created + ChildId
Logs to AuditLogs

If user tries to access dashboard / tracking / community without profile
→ 403 Forbidden + message: «يرجى إكمال ملف طفلك أولاً»
“Please complete your child’s profile first”

Phase 3 – Daily Core Loop (What most mothers do every day)

Logs in (email/password or refresh token)
JWT validated → session active
Lands on personalized dashboard

Sees:
Today’s date + «كيف كان اليوم مع [Nickname]؟»
“How was today for [Nickname]?”
Quick mood picker for mother (stress 1–10 + optional emoji)
Button: «أضيفي ملاحظات اليوم» / “Add Today’s Notes”

Daily Tracking Entry
Form sections: Sleep, Eating, Meltdowns, Communication, Positive Moments, Sensory, Strategies Tried, Strategies Helped
Tags pre-populated from child profile (e.g. common challenges)
Submit
Backend:
POST /api/tracking
Checks ChildId belongs to the authenticated user
Enforces one entry per day (unique ChildId + Date)
Saves record (JSON for tags/sections)
Returns success message + gentle phrase («شكراً لتتبعك اليوم ❤️»)

View History
Calendar or list view: past days
Click day → see full details
Backend: GET /api/tracking?childId=xxx&from=…&to=…
Video Moment Upload (optional but encouraged)
“Upload a short video of a calm moment today”
Drag-drop or select file (frontend enforces < 5 min)
Add private note (e.g. «أحب البطانية الموزونة»)
Backend:
POST /api/videos/upload → returns presigned URL (Azure Blob / S3)
Frontend uploads directly to storage
After upload → POST callback with BlobUrl
Inserts VideoUploads record (Status=Uploaded)
Publishes event “VideoUploaded” (queue)
Mother sees: «تم استلام الفيديو – سنعالجه قريباً»


Phase 4 – Resource Discovery & Inspiration

Navigates to “Resources” tab
Categories: Visual Schedules, Calm Corner Ideas, Play Activities, Mother Self-Care, Recommended Books/Channels
Search or browse cards
Click → view description + link / printable / download
Backend: GET /api/resources?category=VisualSchedules (etc.)

Phase 5 – Community Interaction (Social Safety Layer)

Goes to “Community” tab
Sees feed of Approved posts (latest first, paginated)
Each post shows: content, photo (if any), reactions count, comment count, time ago

Wants to post
“Share something” button
Text area + optional photo
Strong reminder: «احتفظي به عملياً وعاطفياً – لا مصطلحات طبية»
Submit → «منشورك قيد المراجعة – شكراً لمشاركتك بأمان ❤️»
Backend:
POST /api/posts
Keyword scan (regex blocklist)
If clean → Status = Pending
If blocked → reject + message
Stores photo in blob if provided
Logs action

Interacts with others’ posts
React (Heart / Hug / Pray / ThumbsUp) → immediate
Comment → goes to Pending
Report button (if something feels off)
Backend:
POST /api/reactions (immediate)
POST /api/comments (Pending)
POST /api/reports → adds to moderation queue


Phase 6 – Moderator / Admin Side (Parallel Flow – Not Visible to Mothers)

Moderator logs in (role = Moderator / Admin)
Sees moderation queue: Pending posts / comments
Reviews each → Approve / Reject + optional note
Backend:
GET /api/admin/queue
PUT /api/posts/{id}/moderate {Status, Note}
Approved → visible to everyone
Rejected → hidden + reason logged
Full audit log entry for every moderation decision


Phase 7 – Repeated Daily Cycle (Long-term Usage)

Every day: login → dashboard → add tracking entry → (maybe) upload video → browse resources → read/respond in community
Weekly/monthly: check history → see gentle patterns (via external AI – mother sees only safe text result)
Receives daily encouragement tip (in-app banner or notification)

Backend background:

Scheduled job triggers tip request to external AI
Displays returned safe message

Main Navigation & Flow Summary
textLanding → Register → Disclaimer (mandatory) → Create Child Profile (one only)
          ↓
     Dashboard (daily hub)
     ├── Today’s Tracking Entry
     ├── View Past Days
     ├── Upload Video Moment
     ├── Resources Library
     └── Community Feed
          ├── Read Approved Posts
          ├── React / Comment (pending moderation)
          └── Create New Post (pending moderation)
This version should be significantly easier to read and follow.
If you want an even more compact version, a table view for certain phases, or a Mermaid diagram version instead, just let me know.2.6sFastgive me readme file contain the prd of all sides of projectHere is a clean, professional README.md file that contains the full Product Requirements Document (PRD) for Project Autism, covering all sides of the project (user flow, functional requirements, safety layers, community moderation, technical stack, non-functional aspects, etc.).
You can copy-paste it directly into your project root.
Markdown# Project Autism

**Safe Daily Support Platform for Mothers of Children with Autism**  
Mobile-first • Egypt-focused • Bilingual (Arabic/English) • Safety-First Design

Current date reference: March 2026  
Primary audience: Mothers in Egypt (especially Mansoura / Dakahlia region and similar areas)

## Core Principle – Repeated on Every Important Screen

> **This is NOT medical software.**  
> It is a practical daily support tool created with love for mothers.  
> No diagnoses, no treatments, no medical advice.

## 1. Product Overview

**Project Autism** is a secure web application built **exclusively for mothers** to:

- Track simple daily observations about their child (sleep, eating, calm moments, etc.)
- Upload short calm video moments (private)
- Discover practical, non-medical resources
- Participate in a **heavily moderated**, emotionally supportive community
- Receive gentle daily encouragement messages

**Key constraints / safety philosophy**

- One child per mother account (strict rule)
- Mandatory, non-skippable legal disclaimer acceptance
- All community content starts **hidden** (Pending) → human moderation required
- No medical language allowed (keyword blocking + manual review)
- Quick daily usage (2–5 minutes) — not long sessions
- Core system is **non-AI**; external AI used only for safe text generation (tips / patterns)

## 2. Complete User Flow – Deep Version (Mother’s Perspective)

### Phase 0 – Anonymous Visitor
1. Opens website (mobile browser – most likely Egypt)
   - Strong repeated disclaimer visible
   - Arabic / English toggle (full RTL support)
   - Big button: «ابدئي بدعم طفلي» → Register

   **Backend:** static content only (no auth)

### Phase 1 – Onboarding & Disclaimer (Critical Safety Gate)
2. Clicks Register → Email + Password
3. Forced disclaimer screen (non-skippable)
   - Long clear text + checkbox
   - “Confirm” acts as digital signature

   **Backend**
   - Create user (role=Mother, IsActive=false)
   - On accept: set DisclaimerAcceptedAt, IsActive=true
   - Return JWT
   - Log AuditLogs: DisclaimerAccepted
   - Skip/close → session killed

### Phase 2 – Child Profile (Gate to All Features)
4. Redirect → Create Child Profile (one child only)
   - Required: Nickname, AgeYears, AgeMonths
   - Optional: gender, needs level, challenges (tags), strengths, preferences

   **Backend**
   - POST /api/children
   - 409 Conflict if already exists
   - Insert with UserId FK → return 201 + ChildId

5. Any access to dashboard/tracking/community without profile  
   → 403 + message: «يرجى إكمال ملف طفلك أولاً»

### Phase 3 – Daily Core Loop
6. Login → personalized dashboard
7. See: Today’s date + «كيف كان اليوم مع [Nickname]؟»
   - Quick mother stress picker (1–10 + emoji)
   - Button: «أضيفي ملاحظات اليوم»

8. Daily Tracking
   - Sections: Sleep, Eating, Meltdowns, Communication, Positive Moments, Sensory, Strategies…
   - Tags pre-filled from profile

   **Backend**
   - POST /api/tracking
   - One entry per day (ChildId + Date unique)
   - JSON payload
   - Success + gentle message («شكراً لتتبعك اليوم ❤️»)

9. History: calendar / list view  
   GET /api/tracking?childId=…&from=…&to=…

10. Video Upload (encouraged)
    - < 5 min, drag-drop, private note

    **Backend**
    - POST /api/videos/upload → presigned URL
    - Direct upload to Azure Blob / S3
    - Insert VideoUploads record
    - Queue event “VideoUploaded”

### Phase 4 – Resources
11. Resources tab
    - Categories: Visual Schedules, Calm Corners, Play, Mother Self-Care, Books…
    - Cards → description + printable/download

    **Backend** GET /api/resources?category=…

### Phase 5 – Community (Social Safety Layer)
12. Feed: **only Approved** posts (latest, paginated)
13. Create post
    - Text + optional photo
    - Reminder: no medical terms

    **Backend**
    - POST /api/posts
    - Keyword blocklist scan
    - Status = Pending (hidden)
    - Log action

14. Interact
    - Reactions → immediate
    - Comments → Pending
    - Report → moderation queue

    **Backend**
    - POST /api/reactions, /api/comments (Pending), /api/reports

### Phase 6 – Moderator / Admin (Parallel – Invisible to Mothers)
15. Moderator dashboard
    - Queue: Pending posts & comments
    - Approve / Reject + note

    **Backend**
    - GET /api/admin/queue
    - PUT /api/posts/{id}/moderate {Status, Note}
    - Full audit logging

### Phase 7 – Long-term Daily Cycle
- Daily: login → track → maybe video → resources → community
- Weekly/monthly: history patterns (external AI → safe text only)
- Daily tip (scheduled job → external AI → safe output)

## 3. Main Navigation Summary
Landing → Register → Disclaimer (mandatory) → Child Profile (one only)
↓
Dashboard (daily hub)
├── Today’s Tracking
├── View Past Days
├── Upload Video Moment
├── Resources Library
└── Community Feed
├── Read Approved Posts
├── React / Comment (pending moderation)
└── Create New Post (pending moderation)
text## 4. Technical Stack

| Layer              | Technology                        | Notes                                      |
|--------------------|-----------------------------------|--------------------------------------------|
| Backend            | ASP.NET Core 8 (C#)               | REST API, JWT, dependency injection        |
| Database           | Microsoft SQL Server              | JSON columns, full-text, audit tables      |
| Frontend           | JavaScript + React.js / Next.js   | Mobile-first, RTL support (Arabic priority)|
| ORM                | Entity Framework Core 8           | Code-first migrations                      |
| Authentication     | JWT + Refresh tokens              | ASP.NET Core Identity integration          |
| File Storage       | Azure Blob Storage                | Presigned URLs for videos/photos           |
| Background Jobs    | Hangfire (SQL Server backed)      | Daily tips, video post-processing queue    |
| Styling            | Tailwind CSS + DaisyUI            | RTL-ready, accessible                      |
| Hosting            | Azure App Service (Windows)       | Native .NET + SQL support                  |
| API Docs           | Swagger / OpenAPI                 | Auto-generated                             |

## 5. Key Safety & Compliance Layers

- Repeated strong disclaimers
- Non-skippable acceptance (digital signature)
- One-child-per-account rule
- Keyword blocklist + human moderation
- All posts/comments start **Pending** (invisible)
- Full audit trail (every important action logged)
- No PII sharing in community
- Report + block mechanism

## 6. Out of Scope (for v1)

- Multi-child support
- Native mobile apps (PWA sufficient)
- Medical / diagnostic features
- Video analysis (only storage + private note)

## 7. Implementation Priority (MVP → later phases)

1. Landing + Register + Disclaimer + Child Profile
2. Dashboard + Daily Tracking + History
3. Video upload
4. Resources library
5. Community posting + feed (with moderation)
6. Moderator queue + Approve/Reject
7. Daily tips + gentle patterns (external safe AI)

## 8. Final Note

This document describes **the real user journey depth** — everything is protected behind safety gates (disclaimer + profile), focused on **one child**, and designed for **quick daily check-ins** rather than long sessions.

Created with care for Aya and all incredible mothers in Egypt ❤️

Last updated: March 2026
Feel free to ask for:

Arabic-only version of this README
Mermaid diagrams added
Folder structure suggestion
Example database tables (SQL)
API endpoint list in table format

Just tell me what you need next.2.8sFastedit readme file by adding Account for mother then she can make child profileHere is an updated README.md that clearly emphasizes the account creation for the mother before she can create the child's profile.
The structure keeps the previous content but makes the account creation step more prominent and explicit in multiple places (overview, flow, safety layers).
Markdown# Project Autism

**Safe Daily Support Platform for Mothers of Children with Autism**  
Mobile-first • Egypt-focused • Bilingual (Arabic/English) • Safety-First Design

**Current reference date:** March 2026  
**Primary users:** Mothers in Egypt (especially Mansurah / Dakahlia and similar regions)

## Core Principle – Shown on Every Critical Screen

> **This is NOT medical software.**  
> It is a practical daily support tool created with love for mothers.  
> No diagnoses, no treatments, no medical advice.

## 1. Product Overview

**Project Autism** is a secure web platform built **exclusively for mothers** to:

- Create a personal account (Email + Password)
- Accept a strong legal disclaimer (mandatory, non-skippable)
- Create **one** child profile (strict one-child-per-account rule)
- Track simple daily observations (sleep, eating, calm moments, etc.)
- Upload short private calm video moments
- Discover practical, non-medical resources
- Participate in a **heavily moderated** emotional support community
- Receive gentle daily encouragement messages

**Most important safety rules**

- Mother must create an **account** → accept disclaimer → only then can create child profile
- One child per mother account (enforced at database level)
- All community content starts **hidden** (Pending) until human moderator approval
- No medical language allowed (keyword blocking + manual review)
- Designed for quick daily usage (2–5 minutes)

## 2. Complete User Flow – Deep Version (Mother’s Perspective)

### Phase 0 – Anonymous Visitor
1. Opens website (mobile browser – most likely in Egypt)
   - Very strong repeated disclaimer visible
   - Arabic / English toggle (full RTL support)
   - Big button: «ابدئي بدعم طفلي» / “Start Supporting My Child” → Register

   **Backend:** static/public content only (no authentication required)

### Phase 1 – Account Creation + Disclaimer (Critical Safety Gate)

2. Clicks **Register**  
   → Mother creates her personal account

   - Form: Email + Password
   - Immediately after submit → forced disclaimer screen

3. Disclaimer Acceptance (non-skippable – acts as legal agreement)

   - Long clear text explaining the core principle
   - Checkbox: «أفهم تماماً وأقبل أن هذا ليس برنامجاً طبياً...»  
     “I fully understand and accept that this is NOT medical software…”
   - “Confirm” button (digital signature equivalent)

   **Backend**
   - Creates User record (role = Mother, IsActive = false)
   - On confirm: sets DisclaimerAcceptedAt = now, IsActive = true
   - Returns JWT token
   - Logs to AuditLogs: action=DisclaimerAccepted
   - If user closes or tries to skip → session terminated, must start over

**Important:** Only after creating an account and accepting the disclaimer can the mother proceed to create a child profile.

### Phase 2 – First-Time Setup (Child Profile – Gate to All Features)

4. After disclaimer acceptance → redirected to **“Create Your Child’s Profile”**

   - **Required fields:** Nickname, AgeYears, AgeMonths
   - **Optional fields:** Gender, SupportNeedsLevel (Light/Medium/High), Challenges (multi-select tags), Strengths (text), VisualSchedulePref, CommunicationMethods (tags)
   - “Save & Continue” button

   **Backend**
   - POST /api/children
   - Validates required fields
   - Enforces **one-child rule**: if user already has a profile → 409 Conflict
   - Inserts record with UserId foreign key
   - Returns 201 Created + ChildId
   - Logs to AuditLogs

5. If mother tries to access dashboard, tracking, resources or community **without** a child profile  
   → 403 Forbidden + message:  
   «يرجى إكمال ملف طفلك أولاً»  
   “Please complete your child’s profile first”

### Phase 3 – Daily Core Loop (Most Common Daily Usage)

6. Mother logs in (email/password or refresh token)  
   → JWT validated → session active → personalized dashboard

7. Dashboard shows:
   - Today’s date + «كيف كان اليوم مع [Nickname]؟»
   - Quick mother mood picker (stress 1–10 + optional emoji)
   - Button: «أضيفي ملاحظات اليوم»

8. Daily Tracking Entry
   - Sections: Sleep, Eating, Meltdowns, Communication, Positive Moments, Sensory, Strategies Tried, Strategies Helped
   - Tags pre-populated from child profile
   - Submit

   **Backend**
   - POST /api/tracking
   - Verifies ChildId belongs to authenticated user
   - Enforces one entry per day (unique ChildId + Date)
   - Saves data (JSON format)
   - Returns success + gentle message («شكراً لتتبعك اليوم ❤️»)

9. View History  
   GET /api/tracking?childId=…&from=…&to=…

10. Upload Video Moment (optional, encouraged)
    - Short video (<5 min enforced by frontend)
    - Optional private note

    **Backend**
    - POST /api/videos/upload → presigned URL
    - Direct upload to storage (Azure Blob / S3)
    - Callback → insert VideoUploads record
    - Queue event for processing

### Phase 4 – Resources
11. Resources tab  
    GET /api/resources?category=…

### Phase 5 – Community (Moderated Social Layer)
12–15. Feed of approved posts → create post (Pending) → reactions (immediate) → comments/reports (Pending)

### Phase 6 – Moderator/Admin Side
16. Moderation queue → Approve / Reject → full audit logging

### Phase 7 – Long-term Daily Cycle
Daily check-in + weekly/monthly gentle patterns + daily encouragement tip

## 3. Main Navigation & Flow Summary
Anonymous → Register (create mother account) → Accept Disclaimer (mandatory)
↓
Create Child Profile (only one allowed – strict rule)
↓
Dashboard (daily hub)
├── Today’s Tracking Entry
├── View Past Days
├── Upload Video Moment
├── Resources Library
└── Community Feed
├── Read Approved Posts
├── React / Comment (pending moderation)
└── Create New Post (pending moderation)
text## 4. Technical Stack

| Layer            | Technology                     |
|------------------|--------------------------------|
| Backend          | ASP.NET Core 8 (C#)            |
| Database         | Microsoft SQL Server           |
| Frontend         | JavaScript + React.js / Next.js|
| ORM              | Entity Framework Core 8        |
| Auth             | JWT + Refresh tokens           |
| Storage          | Azure Blob Storage             |
| Jobs             | Hangfire                       |
| Styling          | Tailwind CSS + DaisyUI         |
| Hosting          | Azure App Service              |

## 5. Key Safety Rules (Repeated for Emphasis)

- Mother **must create account** first
- Disclaimer **must be accepted** before child profile creation
- One child per account (enforced)
- All posts/comments start **Pending**
- Keyword blocklist + human moderation
- Full audit logging of every important action

Created with care for Aya and all incredible mothers in Egypt ❤️

Last updated: March 2026
This version makes it very clear that:

The mother first creates her own account
Then accepts the disclaimer
Only after that can she create the child profile

Let me know if you want to add more emphasis, change wording, or include Arabic titles/sections in the README.3.5sFastIt is Ai Web app for mothers with autsitic child cotain ai chat bot and Progress tracking . add this to readme fileHere is the updated README.md file incorporating the new features:

The app is now explicitly described as an AI-powered web app for mothers of children with autism.
Added AI Chatbot (with strong safety disclaimers, non-medical role, ethical boundaries).
Added AI-powered Progress Tracking (gentle patterns, insights from history data — external AI only, mother sees safe text only).
Kept the original non-AI core flow (tracking, community, resources) while integrating AI enhancements.
Strengthened disclaimers and safety notes based on ethical best practices.

Markdown# Project Autism

**AI-Powered Daily Support Web App for Mothers of Children with Autism**  
Mobile-first • Egypt-focused • Bilingual (Arabic/English) • Safety-First Design

**Current reference date:** March 2026  
**Primary users:** Mothers in Egypt (especially Al Mansurah, Dakahlia and similar regions)

## Core Principle – Repeated on Every Critical Screen

> **This is NOT medical software. It is NOT a substitute for professional therapy, diagnosis, or medical care.**  
> It is a practical daily support tool created with love for mothers — including gentle AI assistance only.  
> The AI chatbot provides general encouragement, ideas, and reflections — never medical advice.  
> Always consult qualified professionals for health, behavior, or developmental concerns.

## 1. Product Overview

**Project Autism** is a secure, **AI-enhanced** web application built exclusively for mothers to support their autistic children through:

- **Mandatory mother account creation** + strong disclaimer acceptance
- One child profile (strict rule)
- Daily manual tracking (sleep, eating, moods, strategies…)
- Private short video uploads of calm moments
- Practical, non-medical resource library
- Heavily moderated emotional support community
- **AI Chatbot** — safe, empathetic companion for daily encouragement, answering simple parenting questions, suggesting calm-down ideas (non-medical, always redirects to professionals when needed)
- **AI-Powered Progress Tracking** — analyzes historical tracking data (external safe AI) → generates gentle, positive text insights & patterns (e.g. "Lately more calm moments on days with visual schedule — great observation! ❤️") — never diagnosis or medical interpretation

**Key Safety & Ethics Rules**

- Strong, repeated disclaimers everywhere — AI is **not** a therapist
- AI chatbot: limited scope, crisis redirection, no false empathy, no medical claims
- All community content: human moderation + keyword blocks
- One child per account
- Quick daily usage (2–5 min core loop)

## 2. Complete User Flow – Deep Version (Mother’s Perspective)

### Phase 0 – Anonymous Visitor
1. Opens website (mobile browser – most likely Egypt)  
   → Strong repeated disclaimer + AR/EN toggle (RTL)  
   → Big button: «ابدئي بدعم طفلي» → Register

   **Backend:** static content only

### Phase 1 – Account Creation + Disclaimer (Critical Safety Gate)
2. Mother creates account: Email + Password  
3. Forced non-skippable disclaimer screen  
   → Checkbox + "Confirm" (digital signature)

   **Backend:** Create user → JWT → Audit log → IsActive=true only after acceptance

### Phase 2 – Child Profile (Gate to All Features)
4. Redirect → Create one child profile  
   Required: Nickname, AgeYears, AgeMonths  
   Optional: challenges, strengths, preferences…

   **Backend:** POST /api/children → 409 if already exists

5. No profile → 403 on dashboard/tracking/community

### Phase 3 – Daily Core Loop
6. Login → Personalized dashboard  
   → Today + «كيف كان اليوم مع [Nickname]؟»  
   → Mother mood picker (1–10 + emoji)

7. Daily Tracking Entry → POST /api/tracking (one per day)

8. View History → calendar/list

9. Upload Video (<5 min) → presigned URL + queue

**New: AI Chatbot Access**  
→ Floating or dedicated tab: "Chat with Support Bot"  
→ Safe prompts only (e.g. "Ideas for calm-down after meltdown?")  
→ Responses: empathetic, practical, redirect to pros if serious  
→ Backend: calls external safe AI with strict filters & disclaimers

**New: AI Progress Insights**  
→ Weekly/monthly view in History tab  
→ Button: "See Gentle Patterns"  
→ External AI analyzes tracking history → returns **only safe, positive text** (no diagnosis)  
→ Example: "You've noticed more positive moments when using sensory strategies — keep going! ❤️"

### Phase 4 – Resources
10. Browse categories (Visual Schedules, Mother Self-Care…)

### Phase 5 – Community (Moderated)
11–14. Approved feed → Post (Pending) → Reactions immediate, Comments Pending

### Phase 6 – Moderator/Admin
15–16. Queue → Approve/Reject → Audit logs

### Phase 7 – Long-term Cycle
Daily check-in + AI chatbot for support + AI gentle insights + daily tip banner

## 3. Main Navigation Summary
Anonymous → Register (Mother Account) → Disclaimer (mandatory)
↓
Create Child Profile (one only)
↓
Dashboard (daily hub)
├── Today’s Tracking Entry
├── View Past Days + AI Gentle Patterns
├── Upload Video Moment
├── AI Chatbot (safe support)
├── Resources Library
└── Community Feed (moderated)
├── Read Approved Posts
├── React / Comment (pending)
└── Create New Post (pending)
text## 4. Technical Stack

| Layer              | Technology                        |
|--------------------|-----------------------------------|
| Backend            | ASP.NET Core 8 (C#)               |
| Database           | Microsoft SQL Server              |
| Frontend           | JavaScript + React.js / Next.js   |
| ORM                | Entity Framework Core 8           |
| Auth               | JWT + Refresh tokens              |
| File Storage       | Azure Blob Storage                |
| Background Jobs    | Hangfire                          |
| AI Integration     | External safe AI API (tips, insights, chatbot) |
| Styling            | Tailwind CSS + DaisyUI            |
| Hosting            | Azure App Service                 |

## 5. AI-Specific Safety & Ethics

- **AI Chatbot** — always shows: "I'm not a doctor or therapist. For serious concerns, contact a professional."
- Redirects crisis/self-harm talk → emergency resources
- No storage of sensitive health data in AI prompts
- Progress insights → positive framing only, no predictions or diagnoses
- Human moderation layer for community remains primary safety net

Created with love for Aya and all incredible mothers in Egypt ❤️

Last updated: March 2026
This README now fully reflects the AI-enhanced version while preserving the original safety-focused core flow.
If you want separate sections for AI features, Arabic translation of key parts, or Mermaid diagrams added — let me know!2.3sFastedit format to be more readable and add Attractive imoges
Markdown#  Autism Support

**AI-Powered Daily Support Web App for Mothers of Children with Autism**  

**Primary users:** Mothers in Egypt 

## Core Principle – Repeated on Every Critical Screen

> **This is NOT medical software. It is NOT a substitute for professional therapy, diagnosis, or medical care.**  
> It is a practical daily support tool created with love for mothers — including gentle AI assistance only.  
> The AI chatbot provides general encouragement, ideas, and reflections — never medical advice.  
> Always consult qualified professionals for health, behavior, or developmental concerns.

## 1. Product Overview

**Project Autism** is a secure, **AI-enhanced** web application built exclusively for mothers to support their autistic children through:

- **Mandatory mother account creation** + strong disclaimer acceptance
- One child profile (strict rule)
- Daily manual tracking (sleep, eating, moods, strategies…)
- Private short video uploads of calm moments
- Practical, non-medical resource library
- Heavily moderated emotional support community
- **AI Chatbot** — safe, empathetic companion for daily encouragement, answering simple parenting questions, suggesting calm-down ideas (non-medical, always redirects to professionals when needed)
- **AI-Powered Progress Tracking** — analyzes historical tracking data (external safe AI) → generates gentle, positive text insights & patterns (e.g. "Lately more calm moments on days with visual schedule — great observation! ❤️") — never diagnosis or medical interpretation

**Key Safety & Ethics Rules**

- Strong, repeated disclaimers everywhere — AI is **not** a therapist
- AI chatbot: limited scope, crisis redirection, no false empathy, no medical claims
- All community content: human moderation + keyword blocks
- One child per account
- Quick daily usage (2–5 min core loop)

## 2. Complete User Flow – Deep Version (Mother’s Perspective)

### Phase 0 – Anonymous Visitor
1. Opens website (mobile browser – most likely Egypt)  
   → Strong repeated disclaimer + AR/EN toggle (RTL)  
   → Big button: «ابدئي بدعم طفلي» → Register


### Phase 1 – Account Creation + Disclaimer (Critical Safety Gate)
2. Mother creates account: Email + Password  
3. Forced non-skippable disclaimer screen  
   → Checkbox + "Confirm" (digital signature)


### Phase 2 – Child Profile (Gate to All Features)
4. Redirect → Create one child profile  
   Required: Nickname, AgeYears, AgeMonths  
   Optional: challenges, strengths, preferences…


5. No profile → 403 on dashboard/tracking/community

### Phase 3 – Daily Core Loop
6. Login → Personalized dashboard  
   → Today + «كيف كان اليوم مع [Nickname]؟»  
   → Mother mood picker (1–10 + emoji)

7. Daily Tracking Entry (one per day)

8. View History → calendar/list

9. Upload Video (<5 min) → presigned URL + queue

**New: AI Chatbot Access**  
→ Floating or dedicated tab: "Chat with Support Bot"  
→ Safe prompts only (e.g. "Ideas for calm-down after meltdown?")  
→ Responses: empathetic, practical, redirect to pros if serious  
→ Backend: calls external safe AI with strict filters & disclaimers

**New: AI Progress Insights**  
→ Weekly/monthly view in History tab  
→ Button: "See Gentle Patterns"  
→ External AI analyzes tracking history → returns **only safe, positive text** (no diagnosis)  
→ Example: "You've noticed more positive moments when using sensory strategies — keep going! ❤️"

### Phase 4 – Resources
10. Browse categories (Visual Schedules, Mother Self-Care…)

### Phase 5 – Community (Moderated)
11–14. Approved feed → Post (Pending) → Reactions immediate, Comments Pending

### Phase 6 – Admin
15–16. Queue → Approve/Reject → Audit logs

### Phase 7 – Long-term Cycle
Daily check-in + AI chatbot for support + AI gentle insights + daily tip banner

## 3. Main Navigation Summary

Anonymous → Register (Mother Account) → Disclaimer (mandatory)
↓
Create Child Profile (one only)
↓
Dashboard (daily hub)
├── Today’s Tracking Entry
├── View Past Days + AI Gentle Patterns
├── Upload Video Moment
├── AI Chatbot (safe support)
├── Resources Library
└── Community Feed (moderated)
├── Read Approved Posts
├── React / Comment (pending)
└── Create New Post (pending)



## 4. Technical Stack

| Layer              | Technology                        |
|--------------------|-----------------------------------|
| Backend            | ASP.NET Core 8 (C#)               |
| Database           | Microsoft SQL Server              |
| Frontend           | JavaScript + React.js / Next.js   |
| ORM                | Entity Framework Core 8           |
| Auth               | JWT + Refresh tokens              |
| File Storage       | Azure Blob Storage                |
| Background Jobs    | Hangfire                          |
| AI Integration     | External safe AI API (tips, insights, chatbot)             |
| Styling            | Tailwind CSS + DaisyUI            |
| Hosting            | Azure App Service                 |

## 5. AI-Specific Safety & Ethics

- **AI Chatbot** — always shows: "I'm not a doctor or therapist. For serious concerns, contact a professional."
- Redirects crisis/self-harm talk → emergency resources
- No storage of sensitive health data in AI prompts
- Progress insights → positive framing only, no predictions or diagnoses
- Human moderation layer for community remains primary safety net

Created with love for Aya and all incredible mothers in Egypt ❤️

Last updated: March 2026Here's a much more readable, clean, and attractive version of your README with:

Better hierarchy and spacing
Consistent emoji usage for visual appeal & quick scanning
Shorter lines & bullet-friendly formatting
Clear separation between sections
Friendly & warm tone suitable for mothers

Markdown# 🌸 Autism Support – أم دعم التوحد

**AI-Powered Daily Companion Web App for Mothers of Children with Autism**  
💙 Mobile-first • Made with love for Egyptian moms • Arabic + English (RTL ready)

**Primary users** → مخصص للأمهات في مصر ❤️

### ⚠️ Most Important Message – Shown on Every Key Screen

> **هذا البرنامج ليس برنامجًا طبيًا**  
> **This is NOT medical software — not a substitute for doctors, therapists or diagnosis.**  
> مجرد أداة يومية لطيفة للدعم العاطفي والعملي + مساعدة ذكاء اصطناعي آمنة جدًا  
> **الذكاء الاصطناعي لا يقدم نصائح طبية أبدًا** — دائمًا استشيري متخصصًا لأي قلق صحي أو سلوكي.

## 🌟 1. نظرة عامة على التطبيق

تطبيق ويب آمن مصمم خصيصًا للأمهات لمساعدتهن يوميًا في رحلة طفل التوحد:

- حساب أم إلزامي + قبول تنويه قانوني قوي  
- ملف طفل واحد فقط (قاعدة صارمة)  
- تتبع يومي بسيط (نوم – أكل – نوبات – لحظات إيجابية…)  
- رفع فيديوهات قصيرة هادئة (خاصة تمامًا)  
- مكتبة موارد عملية غير طبية  
- مجتمع عاطفي **بإشراف بشري صارم**  
- 🤖 **مساعد ذكاء اصطناعي لطيف** (دردشة آمنة – أفكار تهدئة – تشجيع يومي)  
- 📈 **تحليل تقدم لطيف بالذكاء الاصطناعي** (أنماط إيجابية فقط – بدون تشخيص أبدًا)

**مدة الاستخدام اليومي المعتادة** → ٢ إلى ٥ دقائق فقط

## 🛤️ 2. رحلة المستخدم الكاملة (من وجهة نظر الأم)

### 🚪 المرحلة 0 – الزائرة (قبل التسجيل)

1. تدخل الموقع (غالبًا من الجوال في مصر)  
   → ترى تنويه قوي مكرر + زر تبديل عربي/إنجليزي  
   → زر كبير: **«ابدئي بدعم طفلي»** → تسجيل

### 🔐 المرحلة 1 – إنشاء حساب الأم + التنويه الإلزامي

2. تضغط **تسجيل** → بريد إلكتروني + كلمة مرور  
3. تظهر شاشة التنويه الإلزامية (لا يمكن تخطيها)  
   → نص واضح طويل + مربع اختيار + زر **«أؤكد»**

### 👶 المرحلة 2 – إنشاء ملف الطفل (بوابة كل المميزات)

4. بعد التنويه → تنتقل مباشرة لإنشاء ملف الطفل  
   مطلوب: اللقب – السنوات – الأشهر  
   اختياري: الجنس – التحديات – نقاط القوة – التفضيلات…

5. بدون ملف طفل → لا يمكن الوصول للوحة التحكم أو التتبع أو المجتمع  
   (رسالة: «يرجى إكمال ملف طفلك أولاً»)

### 🌞 المرحلة 3 – الحلقة اليومية الأساسية

6. تسجيل الدخول → لوحة تحكم شخصية  
   → «كيف كان اليوم مع [اسم الطفل]؟»  
   → مقياس مزاج الأم السريع (١–١٠ + إيموجي)

7. **إدخال التتبع اليومي** (مرة واحدة فقط يوميًا)

8. **عرض السجل التاريخي** (تقويم أو قائمة)

9. **رفع لحظة فيديو هادئة** (اختياري – مشجع جدًا)

**جديد 🤖 دردشة مع المساعد الذكي**  
→ أيقونة عائمة أو تبويب «تحدثي مع المساعد»  
→ أسئلة مثل: «أفكار لتهدئة بعد نوبة؟»  
→ ردود داعمة + عملية + توجيه للمتخصص إذا كان الموضوع جدي

**جديد 📊 رؤية الأنماط اللطيفة**  
→ في صفحة السجل → زر «شاهدي الأنماط الإيجابية»  
→ الذكاء الاصطناعي يحلل البيانات → يعطي نصوص إيجابية فقط  
مثال: «لاحظتِ لحظات هدوء أكثر في الأيام التي استخدمتِ فيها الجدول البصري – برافو! 🌟»

### 📚 المرحلة 4 – الموارد

10. تصفح فئات: جداول بصرية – ركن الهدوء – ألعاب – رعاية الأم…

### 💬 المرحلة 5 – المجتمع (بإشراف صارم)

11. رؤية المنشورات الموافق عليها فقط  
12. كتابة منشور → يذهب للمراجعة (Pending)  
13. تفاعل (قلوب – عناق – دعاء…) فوري  
14. تعليق أو إبلاغ → يذهب للإشراف

### 🛡️ المرحلة 6 – لوحة الإشراف (غير مرئية للأمهات)

15–16. المشرف يراجع – يوافق أو يرفض – يسجل كل قرار

### ♻️ المرحلة 7 – الاستخدام اليومي المتكرر

كل يوم: تسجيل دخول → تتبع سريع → دردشة مع المساعد → نظرة على المجتمع → نصيحة يومية صغيرة

## 🗺️ 3. ملخص التنقل الرئيسي
زائرة → تسجيل حساب الأم → قبول التنويه (إلزامي)
↓
إنشاء ملف طفل واحد فقط
↓
لوحة التحكم اليومية 💙
├── تتبع اليوم
├── السجل التاريخي + الأنماط اللطيفة 📊
├── رفع فيديو هادئ
├── دردشة مع المساعد الذكي 🤖
├── مكتبة الموارد 📚
└── المجتمع الآمن 💬
├── قراءة المنشورات الموافق عليها
├── تفاعل / تعليق (قيد المراجعة)
└── كتابة منشور جديد (قيد المراجعة)
text## 🛠️ 4. التقنيات المستخدمة

| الطبقة              | التقنية                             | 
|---------------------|--------------------------------------|
| الخادم (Backend)    | ASP.NET Core 8 (C#)                 |
| قاعدة البيانات      | Microsoft SQL Server                |
| الواجهة الأمامية    | JavaScript + React.js / Next.js     |
| الربط مع قاعدة البيانات | Entity Framework Core 8         |
| المصادقة            | JWT + Refresh tokens                |
| تخزين الملفات       | Azure Blob Storage                  |
| المهام الخلفية       | Hangfire                            |
| الذكاء الاصطناعي     | واجهة خارجية آمنة (نصائح + دردشة + أنماط) |
| التصميم             | Tailwind CSS + DaisyUI              |
| الاستضافة           | Azure App Service                   |

## 🛡️ 5. أمان الذكاء الاصطناعي (مهم جدًا)

- الدردشة دائمًا تعرض: «أنا لست طبيبة ولا معالجة – لأي قلق جدي استشيري متخصصًا»  
- توجيه فوري لأرقام الطوارئ إذا ظهرت كلمات خطر  
- لا يتم حفظ بيانات حساسة في طلبات الذكاء الاصطناعي  
- الأنماط إيجابية فقط – بدون تنبؤات أو تشخيص  
- الإشراف البشري على المجتمع هو الطبقة الأساسية للسلامة

---

**صُنع بحب كبير لآية ولكل أم رائعة في مصر** 🌷💜  
آخر تحديث: مارس ٢٠٢٦
This version is:

Easier to read on mobile
Visually warmer and more inviting with appropriate emojis
Better organized with clear phase icons
Still contains all critical safety messages
Looks friendly and trustworthy for mothers

Let me know if you'd like even more emojis, Arabic titles on every section, or a shorter version! 🌸3sFastno dont change languageHere is the cleaned-up, fully English version of the README file based on the provided deep project flow document.
No Arabic was added or kept in this version (as per your instruction: "no dont change language" → keeping everything in English).
I also made it more readable with better formatting, consistent structure, attractive emojis, clear phase separation, and short lines.
Markdown# 🌸 Autism Support

**AI-Powered Daily Companion Web App for Mothers of Children with Autism**  
Mobile-first • Egypt-focused • Bilingual (Arabic/English with RTL) • Safety-First

**Primary users** → Mothers in Egypt ❤️

### ⚠️ Core Principle – Shown on Every Important Screen

> **This is NOT medical software.**  
> It is **NOT** a substitute for professional therapy, diagnosis, or medical care.  
> A gentle daily support tool created with love for mothers — including safe AI help only.  
> The AI chatbot gives encouragement, ideas & reflections — **never medical advice**.  
> Always consult qualified professionals for any health, behavior or development concerns.

## 🌟 1. Product Overview

A secure web app made **exclusively for mothers** to support their autistic children every day:

- 🤝 Mandatory mother account + strong legal disclaimer acceptance  
- 👶 One child profile only (strict rule)  
- 📅 Simple daily tracking (sleep, eating, meltdowns, positive moments…)  
- 🎥 Private short calm video uploads  
- 📚 Practical non-medical resource library  
- 💬 Heavily moderated emotional support community  
- 🤖 **Safe AI Chatbot** – daily encouragement, calm-down ideas, gentle parenting tips  
- 📈 **AI Gentle Progress Insights** – positive patterns from history (safe text only, no diagnosis)

**Daily usage goal** → 2–5 minutes quick check-in

## 🛤️ 2. Complete User Flow – Deep Version (From Mother’s View)

### 🚪 Phase 0 – Anonymous Visitor (Before First Use)

1. Opens website (usually mobile browser in Egypt)  
   → Very strong repeated disclaimer  
   → Clear Arabic/English toggle (full RTL support)  
   → Big button: **“Start Supporting My Child”** → Register

   **Backend:** Serves only static/public content (no login needed)

### 🔐 Phase 1 – Account Creation + Mandatory Disclaimer (Critical Safety Gate)

2. Clicks **Register**  
   → Form: Email + Password

3. Forced disclaimer screen (cannot be skipped)  
   → Long clear text explaining the core principle  
   → Checkbox: “I fully understand and accept that this is NOT medical software…”  
   → **“Confirm”** button (acts like digital signature)

   **Backend:**  
   • Creates User record (role = Mother, IsActive = false)  
   • On confirm → sets DisclaimerAcceptedAt = now, IsActive = true  
   • Returns JWT token  
   • Logs to AuditLogs: action=DisclaimerAccepted

4. If she tries to skip or close → session ends, must start over

### 👶 Phase 2 – First-Time Setup (Child Profile – Gate to All Features)

5. After disclaimer → redirected to **“Create Your Child’s Profile”**  
   • **Required:** Nickname, AgeYears, AgeMonths  
   • **Optional:** Gender, SupportNeedsLevel (Light/Medium/High), Challenges (tags), Strengths (text), preferences…

   **Backend:**  
   • POST /api/children  
   • Validates required fields  
   • One-child rule → 409 Conflict if profile already exists  
   • Inserts record with UserId foreign key  
   • Returns 201 Created + ChildId  
   • Logs to AuditLogs

6. Trying to access dashboard / tracking / community without profile  
   → 403 Forbidden + message: “Please complete your child’s profile first”

### 🌞 Phase 3 – Daily Core Loop (What most mothers do every day)

7. Logs in (email/password or refresh token)  
   → JWT validated → session active → lands on personalized dashboard

8. Dashboard shows:  
   • Today’s date + “How was today for [Nickname]?”  
   • Quick mother mood picker (stress 1–10 + optional emoji)  
   • Button: “Add Today’s Notes”

9. **Daily Tracking Entry** (one per day)  
   • Sections: Sleep, Eating, Meltdowns, Communication, Positive Moments, Sensory, Strategies…  
   • Tags pre-filled from child profile  
   • Submit

   **Backend:**  
   • POST /api/tracking  
   • Verifies ChildId belongs to user  
   • Enforces one entry per day (ChildId + Date unique)  
   • Saves data (JSON)  
   • Returns success + warm message (“Thank you for tracking today ❤️”)

10. **View History**  
    • Calendar or list of past days  
    • Click day → full details

    **Backend:** GET /api/tracking?childId=xxx&from=…&to=…

11. **Video Moment Upload** (optional but encouraged)  
    • “Upload a short video of a calm moment today”  
    • Drag-drop or select (<5 min enforced by frontend)  
    • Optional private note (e.g. “He loved the weighted blanket”)

    **Backend:**  
    • POST /api/videos/upload → returns presigned URL (Azure Blob / S3)  
    • Frontend uploads directly  
    • Callback → inserts VideoUploads record (Status=Uploaded)  
    • Publishes “VideoUploaded” event  
    • Mother sees: “Video received – we’ll process it soon”

### 📚 Phase 4 – Resource Discovery & Inspiration

12. Goes to **“Resources”** tab  
    • Categories: Visual Schedules, Calm Corner Ideas, Play Activities, Mother Self-Care, Books/Channels  
    • Search or browse cards → description + link/print/download

    **Backend:** GET /api/resources?category=…

### 💬 Phase 5 – Community Interaction (Social Safety Layer)

13. Goes to **“Community”** tab  
    • Feed of **Approved** posts only (latest first, paginated)  
    • Each post shows: text, photo (if any), reactions, comments, time ago

14. Wants to post  
    • “Share something” button  
    • Text + optional photo  
    • Reminder: “Keep it practical & emotional – no medical terms”  
    • Submit → “Your post is under review – thank you for sharing safely ❤️”

    **Backend:**  
    • POST /api/posts  
    • Keyword blocklist scan (regex)  
    • Clean → Status = Pending  
    • Blocked → reject + message  
    • Stores photo if provided  
    • Logs action

15. Interacts with posts  
    • React (Heart / Hug / Pray / ThumbsUp) → immediate  
    • Comment → goes to Pending  
    • Report button (if something feels wrong)

    **Backend:**  
    • POST /api/reactions (immediate)  
    • POST /api/comments (Pending)  
    • POST /api/reports → adds to moderation queue

### 🛡️ Phase 6 – Moderator / Admin Side (Not visible to mothers)

16. Moderator logs in (role = Moderator / Admin)  
    • Sees moderation queue: Pending posts & comments  
    • Reviews → Approve / Reject + optional note

    **Backend:**  
    • GET /api/admin/queue  
    • PUT /api/posts/{id}/moderate {Status, Note}  
    • Approved → becomes visible  
    • Rejected → hidden + reason logged  
    • Full audit log for every decision

### ♻️ Phase 7 – Repeated Daily Cycle (Long-term Usage)

Every day:  
→ Login → dashboard → track today → maybe upload video → browse resources → read / interact in community

Weekly / monthly:  
→ Check history → see gentle patterns (external AI → mother sees only safe positive text)

Daily:  
→ Receives small encouragement tip (banner or notification)

**Backend background:**  
• Scheduled job calls external AI for safe tip  
• Displays only filtered, positive message

## 🗺️ Main Navigation & Flow Summary
Anonymous visitor
↓
Register (create mother account)
↓
Accept Disclaimer (mandatory – cannot skip)
↓
Create Child Profile (one child only – strict rule)
↓
Dashboard – Daily Hub 💙
├── Today’s Tracking Entry
├── View Past Days + Gentle AI Patterns 📊
├── Upload Calm Video Moment 🎥
├── Chat with Safe AI Assistant 🤖
├── Resources Library 📚
└── Community Feed (heavily moderated) 💬
├── Read Approved Posts
├── React / Comment (pending moderation)
└── Create New Post (pending moderation)
text
